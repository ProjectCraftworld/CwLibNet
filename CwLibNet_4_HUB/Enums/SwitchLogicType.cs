using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public enum SwitchLogicType
{
    // AND(0)
    AND,
    // OR(1)
    OR,
    // XOR(2)
    XOR,
    // NOT(3)
    NOT,
    // NOP(4)
    NOP
}