﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.WinAPI.pInvoke
{
    public enum SW : uint
    {
        HIDE = 0,
        //Скрыть окно и активизировать другое окно.
        MAXIMIZE = 3,
        //Развернуть окно.
        MINIMIZE = 6,
        //Свернуть окно и активизировать следующее окно в Z-порядке(следующее под свернутым окном).
        RESTORE = 9,
        //Активизировать и отобразить окно.Если окно свернуто или развернуто,Windows восстанавливает его исходный размер и положение.
        SHOW = 5,
        //Активизировать окно.
        SHOWMAXIMIZED = 3,
        //Отобразить окно в развернутом виде.
        SHOWMINIMIZED = 2,
        //Отобразить окно в свернутом виде.
        SHOWMINNOACTIVE = 7,
        //Отобразить окно в свернутом виде.Активное окно остается активным.
        SHOWNA = 8,
        //Отобразить окно в текущем состоянии.Активное окно остается активным.
        SHOWNOACTIVATE = 4,
        //Отобразить окно в соответствии с последними значениями позиции и размера.Активное окно остается активным.
        SHOWNORMAL = 1
        //Активизировать и отобразить окно.Если окно свернуто или развернуто,Windows восстанавливает его исходный размер и положение.Приложение должно указывать этот флаг при первом отображении окна.
    }
}
