namespace Rdp.Core.Data
{
    //新增不限类型的数据比较，用于基于特性的拼接。Add by Tim 2017-03-08
    public enum SqlCompareType
    {
        Equal,
        EqualGreater,
        EqualLess,
        Greater,
        In,
        Less,
        NotEqual
    }

    public enum SqlDataType
    {
        IntBetween,
        IntEqual,
        IntEqualGreater,
        IntEqualLess,
        IntFrom,
        IntGreater,
        IntIn,
        IntLess,
        IntNotEqual,
        IntTo,
        NVarcharBetween,
        NVarcharEqual,
        NVarcharEqualGreater,
        NVarcharEqualLess,
        NVarcharFrom,
        NVarcharGreater,
        NVarcharIn,
        NVarcharLess,
        NVarcharLike,
        NVarcharRightLike,
        NVarcharUpperLike,
        NVarcharNotEqual,
        NVarcharTo,
        VarcharBetween,
        VarcharEqual,
        VarcharEqualGreater,
        VarcharEqualLess,
        VarcharFrom,
        VarcharGreater,
        VarcharIn,
        VarcharLess,
        VarcharLike,
        VarcharRightLike,
        VarcharNotEqual,
        VarcharTo
    }
}

