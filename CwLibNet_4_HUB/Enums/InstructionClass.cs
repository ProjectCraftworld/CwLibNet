using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public enum InstructionClass
{
    NOP,
    LOAD_CONST,
    CAST,
    UNARY,
    BINARY,
    GET_BUILTIN_MEMBER,
    SET_BUILTIN_MEMBER,
    GET_MEMBER,
    SET_MEMBER,
    GET_ELEMENT,
    SET_ELEMENT,
    NEW_OBJECT,
    NEW_ARRAY,
    WRITE,
    ARG,
    CALL,
    RETURN,
    BRANCH
}