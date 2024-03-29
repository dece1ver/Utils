﻿using DetailsList.Infrastructure.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsList.Infrastructure
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum FindMode
    {
        [Description("Обычный")]
        General,
        [Description("Обычный [Только обозначение]")]
        GeneralOnlyNumbers,
        [Description("Mazak QTS 350 [Только обозначение]")]
        Mazak350,
        [Description("Quaser [Только обозначение]")]
        QuaserOnlyNumbers,
        [Description("Имя файла [Только обозначение]")]
        FileName,
        [Description("Имя папки [Только обозначение]")]
        DirName
    }
}
