namespace interfaz {
    class AplitopProperties {
        public const Environment environment = Environment.DEVELOPMENT;

        public static bool isDevelopment() {
            if (environment.Equals(Environment.DEVELOPMENT)) {
                return true;
            }
            return false;
        }
    }

    enum Environment {
        DEVELOPMENT,
        RELEASE
    }
}
