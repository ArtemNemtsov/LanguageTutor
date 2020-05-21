    using System;

namespace MediaStudioService.Core.Classes
{
    public class RespоnceManager
    {
        public static Responce CreateError(string exceptionMessage)
        {
            return new Responce { Success = false, ErrorsMessage = exceptionMessage };
        }

        public static Responce CreateError(Exception ex)
        {
            return new Responce { Success = false, ErrorsMessage = ex.Message + ex.InnerException };
        }

        public static Responce CreateSucces(object body)
        {
            return new Responce { Success = true, Body = body };
        }
    }
}
