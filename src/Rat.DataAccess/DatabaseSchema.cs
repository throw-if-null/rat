namespace Rat.DataAccess
{
    public static class DatabaseSchema
    {
        public static class ProjectSchema
        {
            public const int Max_Name_Length = 248;
        }

        public static class ProjectTypeSchema
        {
            public const int Max_Name_Length = 64;
        }

        public static class UserSchema
        {
            public const int Max_UserId_Length = 128;
        }

		public static class ConfigurationRootSchema
		{
			public const int Max_Name_Length = 248;
		}

		public static class ConfigurationEntrySchema
		{
			public const int Max_Key_Length = 128;
			public const int Max_Value_Length = 4096;
		}

		public static class ConfigurationTypeSchema
		{
			public const int Max_Name_Length = 64;
		}
	}
}
