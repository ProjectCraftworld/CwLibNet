using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

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