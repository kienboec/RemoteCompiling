﻿namespace RestWebservice_RemoteCompiling.JsonObjClasses
{
    public class PistonReturn
    {
        public string stdout { get; set; }
        public string stderr { get; set; }
        public int code { get; set; }
        public string signal { get; set; }
    }
}
