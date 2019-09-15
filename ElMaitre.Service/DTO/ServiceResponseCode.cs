using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DTO
{
    public enum ServiceResponseCode
    {
        BadParameter,
        UnCompleteParamter,
        UnAuthorized,
        Success,
        BadData,
        ServerError,
        NumberIsExist,
        EmailIsExist,
        NumberNotExist,
        ChargeCardNotValid
    }
}
