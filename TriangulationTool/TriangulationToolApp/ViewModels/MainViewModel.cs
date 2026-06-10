using System;
using System.Diagnostics;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using TriangulationToolApp.Business;
using TriangulationToolApp.Views;

namespace TriangulationToolApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int Digits = 3;
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private readonly ViewportLayout _viewportLayout;

        private displayType _displayMode;
        private RelayCommand _exportCommand;
        private RelayCommand _generateSearchTreeCommand;
        private bool _isBusy;
        private RelayCommand _loadFileCommand;
        private FastPointCloud _pointCloud;
        private RelayCommand _triangulateCommand;
        private Mesh _traingulatedMesh;

        public MainViewModel(ViewportLayout viewportLayout, ISnackbarMessageQueue snackbarMessageQueue)
        {
            _viewportLayout = viewportLayout;
            _snackbarMessageQueue = snackbarMessageQueue;
            SceneManager = new SceneManager();
            SetupViewport();
        }

        private Mesh TraingulatedMesh
        {
            get { return _traingulatedMesh; }
            set
            {
                _traingulatedMesh = value;
                GenerateSearchTreeCommand.RaiseCanExecuteChanged();
            }
        }

        public SceneManager SceneManager { get; set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                Set(() => IsBusy, ref _isBusy, value);
                LoadFileCommand.RaiseCanExecuteChanged();
                TriangulateCommand.RaiseCanExecuteChanged();
                ExportCommand.RaiseCanExecuteChanged();
                GenerateSearchTreeCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand LoadFileCommand
        {
            get
            {
                return _loadFileCommand != null
                    ? _loadFileCommand
                    : (_loadFileCommand = new RelayCommand(ExecuteLoadFile, CanExecuteLoadFile));
            }
        }

        public RelayCommand TriangulateCommand
        {
            get
            {
                return _triangulateCommand != null
                    ? _triangulateCommand
                    : (_triangulateCommand = new RelayCommand(ExecuteTriangulate, CanExecuteTriangulate));
            }
        }

        public RelayCommand ExportCommand
        {
            get
            {
                return _exportCommand != null
                    ? _exportCommand
                    : (_exportCommand = new RelayCommand(ExecuteExport, CanExecuteExport));
            }
        }

        public RelayCommand GenerateSearchTreeCommand
        {
            get
            {
                if (_generateSearchTreeCommand == null)
                {
                    _generateSearchTreeCommand = new RelayCommand(ExecuteGenerateSearchTree,
                        CanExecuteGenerateSearchTree);
                }
                return _generateSearchTreeCommand;
            }
        }

        private bool IsMeshCreated
        {
            get { return TraingulatedMesh != null; }
        }

        public displayType DisplayMode
        {
            get { return _displayMode; }
            set { Set(() => DisplayMode, ref _displayMode, value); }
        }

        private async void ExecuteGenerateSearchTree()
        {
            if (CanNotExecuteGenerateSearchTree()) return;

            var view = new ShowInputNumRegionsView();
            var viewModel = new ShowInputNumRegionsViewModel("Introduce el número de nodos por región [rec:500]");
            viewModel.View = view;
            view.DataContext = viewModel;
            view.ShowDialog();

            if (viewModel.Value == -1) return;

            var fileDialog = new SaveFileDialog
            {
                DefaultExt = "tdm",
                Filter = "TADIL MDT File | *.tdm"
            };
            var result = fileDialog.ShowDialog();

            if (result == null || result == false) return;

            IsBusy = true;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var exportOk = await SceneManager.GenerateSearchTreeAsync(TraingulatedMesh, fileDialog.FileName, viewModel.Value);
            stopwatch.Stop();

            IsBusy = false;

            ShowStatusMessage(exportOk
                ? string.Format("File exported to TDM successfully in {0} seconds", Math.Round(stopwatch.Elapsed.TotalSeconds, Digits))
                : "There was an error exporting the file.");
        }

        private bool CanExecuteGenerateSearchTree()
        {
            return !IsBusy && IsMeshCreated;
        }

        private bool CanNotExecuteGenerateSearchTree()
        {
            return !GenerateSearchTreeCommand.CanExecute(null);
        }

        private void SetupViewport()
        {
            DisplayMode = displayType.Shaded;
            SetupViewportEvents();
        }

        private void SetupViewportEvents()
        {
            _viewportLayout.WorkCompleted += OnWorkCompleted;
            _viewportLayout.WorkCancelled += OnWorkCancelled;
            _viewportLayout.WorkFailed += OnWorkFailed;
        }

        private void OnWorkFailed(object sender, WorkFailedEventArgs e)
        {
            ShowStatusMessage(string.Format("Error exporting to Autocad: {0}", e.Error));
            IsBusy = false;
        }

        private void OnWorkCancelled(object sender, EventArgs e)
        {
            ShowStatusMessage("Process canceled");
            IsBusy = false;
        }

        private void OnWorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            ShowStatusMessage(
                string.Format("Operation completed in {0} seconds", Math.Round(TimeSpan.FromMilliseconds(e.WorkUnit.ExecutionTime).TotalSeconds, Digits)));
            IsBusy = false;
        }

        private async void ExecuteLoadFile()
        {
            if (CanNotExecuteLoadFile()) return;

            var fileDialog = new OpenFileDialog {Filter = "CSV Files | *.csv"};
            var result = fileDialog.ShowDialog();

            if (result == null || result == false) return;

            try
            {
                RemovePointCloudFromViewport();
                IsBusy = true;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                _pointCloud = await SceneManager.LoadPointCloudAsync(fileDialog.FileName);
                stopwatch.Stop();
                TriangulateCommand.RaiseCanExecuteChanged();
                string content =
                    string.Format("Point cloud with {0} points loaded in {1} seconds", SceneManager.NumberOfPointsInCloud, Math.Round(stopwatch.Elapsed.TotalSeconds, Digits));
                ShowStatusMessage(content);

                IsBusy = false;

                AddPointCloudToViewport();
                UpdateViewport();
            }
            catch (Exception)
            {
                ShowStatusMessage("Invalid point cloud file");
            }
        }

        private void AddPointCloudToViewport()
        {
            _viewportLayout.Entities.Add(_pointCloud);
        }

        private void RemovePointCloudFromViewport()
        {
            if (_pointCloud != null) _viewportLayout.Entities.Remove(_pointCloud);
        }

        private void ShowStatusMessage(string content)
        {
            _snackbarMessageQueue.Enqueue(content);
        }

        private bool CanExecuteLoadFile()
        {
            return !IsBusy;
        }

        private bool CanNotExecuteLoadFile()
        {
            return !LoadFileCommand.CanExecute(null);
        }

        private async void ExecuteTriangulate()
        {
            if (CanNotExecuteTriangulate()) return;

            IsBusy = true;
            RemoveTriangulatedMeshFromViewport();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TraingulatedMesh = await SceneManager.TriangulatePointCloudAsync();
            TraingulatedMesh.Selected = true;
            TraingulatedMesh.LightWeight = true;
            stopwatch.Stop();
            ExportCommand.RaiseCanExecuteChanged();
            ShowStatusMessage(
                string.Format("Triangulation process completed in {0} seconds", Math.Round(stopwatch.Elapsed.TotalSeconds, Digits)));

            IsBusy = false;

            AddTriangulatedMeshToViewport();
            UpdateViewport();
        }

        private void AddTriangulatedMeshToViewport()
        {
            _viewportLayout.Entities.Add(TraingulatedMesh);
        }

        private void RemoveTriangulatedMeshFromViewport()
        {
            if (TraingulatedMesh != null) _viewportLayout.Entities.Remove(TraingulatedMesh);
        }

        private void UpdateViewport()
        {
            _viewportLayout.ZoomFit();
            _viewportLayout.Invalidate();
        }

        private bool CanExecuteTriangulate()
        {
            return SceneManager.IsPointCloudLoaded && !IsBusy;
        }

        private bool CanNotExecuteTriangulate()
        {
            return !TriangulateCommand.CanExecute(null);
        }

        private async void ExecuteExport()
        {
            if (CanNotExecuteExport()) return;

            var fileDialog = new SaveFileDialog
            {
                DefaultExt = "dwg",
                Filter = "Autocad DWG File | *.dwg"
            };
            var result = fileDialog.ShowDialog();

            if (result == null || result == false) return;

            IsBusy = true;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var exportOk = await SceneManager.ExportToAutodeskAsync(_viewportLayout, fileDialog.FileName);
            stopwatch.Stop();

            IsBusy = false;

            ShowStatusMessage(exportOk
                ? string.Format("File exported to DWG successfully in {0} seconds", Math.Round(stopwatch.Elapsed.TotalSeconds, Digits))
                : "There was an error exporting the file.");
        }

        private bool CanExecuteExport()
        {
            return SceneManager.IsMeshTriangulated && !IsBusy;
        }

        private bool CanNotExecuteExport()
        {
            return !ExportCommand.CanExecute(null);
        }
    }
}