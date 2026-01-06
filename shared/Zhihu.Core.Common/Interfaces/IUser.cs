using System;
using System.Collections.Generic;
using System.Text;
using Zhihu.SharedModels.Enums;

namespace Zhihu.Core.Common.Interfaces;

public interface IUser
{
    int? Id { get; }
    UserType UserType { get; }
}