namespace SimpleDataGrid
{
    public class TextManager
    {
        static TextManager()
        {
            page = "page";
            of = "of";
            rows = "rows";
            load = "Load";
            save = "Save";
        }
        
        private static string page;
        public static string Page { get { return page; } set { page = value; } }

        private static string of;
        public static string Of { get { return of; } set { of = value; } }

        private static string rows;
        public static string Rows { get { return rows; } set { rows = value; } }

        private static string load;
        public static string Load { get { return load; } set { load = value; } }

        private static string save;
        public static string Save { get { return save; } set { save = value; } }
    }
}
