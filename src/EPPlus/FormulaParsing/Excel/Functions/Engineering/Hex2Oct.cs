﻿/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  05/03/2020         EPPlus Software AB         Implemented function
 *************************************************************************************************/
using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering.Helpers;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering
{
    internal class Hex2Oct : ExcelFunction
    {
        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
        {
            ValidateArguments(arguments, 1);
            var number = ArgToString(arguments, 0);
            var padding = default(int?);
            if (arguments.Count() > 1)
            {
                padding = ArgToInt(arguments, 1);
                if (padding.Value < 0 ^ padding.Value > 10) return CreateResult(eErrorType.Num);
            }
            var hexToDec = new Hex2Dec();
            var decNumber = (double)hexToDec.Execute(new FunctionArgument[] { arguments.ElementAt(0) }, context).ResultValue;
            var result = Convert.ToString(Convert.ToInt32(decNumber), 8);
            if (decNumber < 0)
            {
                result = PaddingHelper.EnsureLength(result, 10, "7");
            }
            else if (padding.HasValue)
            {
                result = PaddingHelper.EnsureLength(result, padding.Value, "0");
            }
            else
            {
                result = PaddingHelper.EnsureMinLength(result, 10);
            }
            return CreateResult(result, DataType.String);
        }
    }
}