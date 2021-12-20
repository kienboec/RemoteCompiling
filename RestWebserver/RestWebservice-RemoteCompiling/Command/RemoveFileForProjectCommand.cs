﻿namespace RestWebservice_RemoteCompiling.Command
{
    public class RemoveFileForProjectCommand : BaseCommand<bool>
    {
        public int FileId
        {
            get;
            set;
        }

        public int ProjectId
        {
            get;
            set;
        }
    }
}