namespace KLTest
{
    public class FileInfo
    {
        public string Zone;
        public GeneralInfo FileGeneralInfo;

        public class GeneralInfo
        {
            public string FileStatus;
            public string Sha1;
            public string Md5;
            public string Sha256;
            public string FirstSeen;
            public string LastSeen;
            public string Signer;
            public int? Size;
            public string Type;
            public int? HitsCount;
        }

        public bool AllFieldsNotNull()
        {
            if (Zone == null)
                return false;

            if (FileGeneralInfo == null)
                return false;

            if (FileGeneralInfo.FileStatus == null)
                return false;

            if (FileGeneralInfo.Sha1 == null)
                return false;

            if (FileGeneralInfo.Md5 == null)
                return false;

            if (FileGeneralInfo.Sha256 == null)
                return false;

            if (FileGeneralInfo.FirstSeen == null)
                return false;

            if (FileGeneralInfo.LastSeen == null)
                return false;

            if (FileGeneralInfo.Signer == null)
                return false;

            if (FileGeneralInfo.Size == null)
                return false;

            if (FileGeneralInfo.Type == null)
                return false;

            if (FileGeneralInfo.HitsCount == null)
                return false;

            return true;
        }
    }
}
