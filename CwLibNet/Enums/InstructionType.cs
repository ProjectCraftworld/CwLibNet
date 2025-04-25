using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum InstructionType : byte
    {
        // NOP(0x0)
        NOP,
        // LCb(0x1)
        LCb,
        // LCc(0x2)
        LCc,
        // LCi(0x3)
        LCi,
        // LCf(0x4)
        LCf,
        // LCsw(0x5)
        LCsw,
        // LC_NULLsp(0x6)
        LC_NULLsp,
        // MOVb(0x7)
        MOVb,
        // LOG_NEGb(0x8)
        LOG_NEGb,
        // MOVc(0x9)
        MOVc,
        // MOVi(0xa)
        MOVi,
        // INCi(0xb)
        INCi,
        // DECi(0xc)
        DECi,
        // NEGi(0xd)
        NEGi,
        // BIT_NEGi(0xe)
        BIT_NEGi,
        // LOG_NEGi(0xf)
        LOG_NEGi,
        // ABSi(0x10)
        ABSi,
        // MOVf(0x11)
        MOVf,
        // NEGf(0x12)
        NEGf,
        // ABSf(0x13)
        ABSf,
        // SQRTf(0x14)
        SQRTf,
        // SINf(0x15)
        SINf,
        // COSf(0x16)
        COSf,
        // TANf(0x17)
        TANf,
        // MOVv4(0x18)
        MOVv4,
        // NEGv4(0x19)
        NEGv4,
        // MOVm44(0x1a)
        MOVm44,
        // @Deprecated
        // MOVs(0x1b)
        MOVs,
        // MOVrp(0x1c)
        MOVrp,
        // MOVcp(0x1d)
        MOVcp,
        // MOVsp(0x1e)
        MOVsp,
        // MOVo(0x1f)
        MOVo,
        // EQb(0x20)
        EQb,
        // NEb(0x21)
        NEb,
        // RESERVED0(0x22)
        RESERVED0,
        // RESERVED1(0x23)
        RESERVED1,
        // LTc(0x24)
        LTc,
        // LTEc(0x25)
        LTEc,
        // GTc(0x26)
        GTc,
        // GTEc(0x27)
        GTEc,
        // EQc(0x28)
        EQc,
        // NEc(0x29)
        NEc,
        // ADDi(0x2a)
        ADDi,
        // SUBi(0x2b)
        SUBi,
        // MULi(0x2c)
        MULi,
        // DIVi(0x2d)
        DIVi,
        // MODi(0x2e)
        MODi,
        // MINi(0x2f)
        MINi,
        // MAXi(0x30)
        MAXi,
        // SLAi(0x31)
        SLAi,
        // SRAi(0x32)
        SRAi,
        // SRLi(0x33)
        SRLi,
        // BIT_ORi(0x34)
        BIT_ORi,
        // BIT_ANDi(0x35)
        BIT_ANDi,
        // BIT_XORi(0x36)
        BIT_XORi,
        // LTi(0x37)
        LTi,
        // LTEi(0x38)
        LTEi,
        // GTi(0x39)
        GTi,
        // GTEi(0x3a)
        GTEi,
        // EQi(0x3b)
        EQi,
        // NEi(0x3c)
        NEi,
        // ADDf(0x3d)
        ADDf,
        // SUBf(0x3e)
        SUBf,
        // MULf(0x3f)
        MULf,
        // DIVf(0x40)
        DIVf,
        // MINf(0x41)
        MINf,
        // MAXf(0x42)
        MAXf,
        // LTf(0x43)
        LTf,
        // LTEf(0x44)
        LTEf,
        // GTf(0x45)
        GTf,
        // GTEf(0x46)
        GTEf,
        // EQf(0x47)
        EQf,
        // NEf(0x48)
        NEf,
        // ADDv4(0x49)
        ADDv4,
        // SUBv4(0x4a)
        SUBv4,
        // MULSv4(0x4b)
        MULSv4,
        // DIVSv4(0x4c)
        DIVSv4,
        // DOT4v4(0x4d)
        DOT4v4,
        // DOT3v4(0x4e)
        DOT3v4,
        // DOT2v4(0x4f)
        DOT2v4,
        // CROSS3v4(0x50)
        CROSS3v4,
        // MULm44(0x51)
        MULm44,
        // @Deprecated
        // EQs(0x52)
        EQs,
        // @Deprecated
        // NEs(0x53)
        NEs,
        // EQrp(0x54)
        EQrp,
        // NErp(0x55)
        NErp,
        // EQo(0x56)
        EQo,
        // NEo(0x57)
        NEo,
        // EQsp(0x58)
        EQsp,
        // NEsp(0x59)
        NEsp,
        // GET_V4_X(0x5a)
        GET_V4_X,
        // GET_V4_Y(0x5b)
        GET_V4_Y,
        // GET_V4_Z(0x5c)
        GET_V4_Z,
        // GET_V4_W(0x5d)
        GET_V4_W,
        // GET_V4_LEN2(0x5e)
        GET_V4_LEN2,
        // GET_V4_LEN3(0x5f)
        GET_V4_LEN3,
        // GET_V4_LEN4(0x60)
        GET_V4_LEN4,
        // GET_M44_XX(0x61)
        GET_M44_XX,
        // GET_M44_XY(0x62)
        GET_M44_XY,
        // GET_M44_XZ(0x63)
        GET_M44_XZ,
        // GET_M44_XW(0x64)
        GET_M44_XW,
        // GET_M44_YX(0x65)
        GET_M44_YX,
        // GET_M44_YY(0x66)
        GET_M44_YY,
        // GET_M44_YZ(0x67)
        GET_M44_YZ,
        // GET_M44_YW(0x68)
        GET_M44_YW,
        // GET_M44_ZX(0x69)
        GET_M44_ZX,
        // GET_M44_ZY(0x6a)
        GET_M44_ZY,
        // GET_M44_ZZ(0x6b)
        GET_M44_ZZ,
        // GET_M44_ZW(0x6c)
        GET_M44_ZW,
        // GET_M44_WX(0x6d)
        GET_M44_WX,
        // GET_M44_WY(0x6e)
        GET_M44_WY,
        // GET_M44_WZ(0x6f)
        GET_M44_WZ,
        // GET_M44_WW(0x70)
        GET_M44_WW,
        // GET_M44_rX(0x71)
        GET_M44_rX,
        // GET_M44_rY(0x72)
        GET_M44_rY,
        // GET_M44_rZ(0x73)
        GET_M44_rZ,
        // GET_M44_rW(0x74)
        GET_M44_rW,
        // GET_M44_cX(0x75)
        GET_M44_cX,
        // GET_M44_cY(0x76)
        GET_M44_cY,
        // GET_M44_cZ(0x77)
        GET_M44_cZ,
        // GET_M44_cW(0x78)
        GET_M44_cW,
        // SET_V4_X(0x79)
        SET_V4_X,
        // SET_V4_Y(0x7a)
        SET_V4_Y,
        // SET_V4_Z(0x7b)
        SET_V4_Z,
        // SET_V4_W(0x7c)
        SET_V4_W,
        // SET_M44_XX(0x7d)
        SET_M44_XX,
        // SET_M44_XY(0x7e)
        SET_M44_XY,
        // SET_M44_XZ(0x7f)
        SET_M44_XZ,
        // SET_M44_XW(0x80)
        SET_M44_XW,
        // SET_M44_YX(0x81)
        SET_M44_YX,
        // SET_M44_YY(0x82)
        SET_M44_YY,
        // SET_M44_YZ(0x83)
        SET_M44_YZ,
        // SET_M44_YW(0x84)
        SET_M44_YW,
        // SET_M44_ZX(0x85)
        SET_M44_ZX,
        // SET_M44_ZY(0x86)
        SET_M44_ZY,
        // SET_M44_ZZ(0x87)
        SET_M44_ZZ,
        // SET_M44_ZW(0x88)
        SET_M44_ZW,
        // SET_M44_WX(0x89)
        SET_M44_WX,
        // SET_M44_WY(0x8a)
        SET_M44_WY,
        // SET_M44_WZ(0x8b)
        SET_M44_WZ,
        // SET_M44_WW(0x8c)
        SET_M44_WW,
        // SET_M44_rX(0x8d)
        SET_M44_rX,
        // SET_M44_rY(0x8e)
        SET_M44_rY,
        // SET_M44_rZ(0x8f)
        SET_M44_rZ,
        // SET_M44_rW(0x90)
        SET_M44_rW,
        // SET_M44_cX(0x91)
        SET_M44_cX,
        // SET_M44_cY(0x92)
        SET_M44_cY,
        // SET_M44_cZ(0x93)
        SET_M44_cZ,
        // SET_M44_cW(0x94)
        SET_M44_cW,
        // GET_SP_MEMBER(0x95)
        GET_SP_MEMBER,
        // GET_RP_MEMBER(0x96)
        GET_RP_MEMBER,
        // SET_SP_MEMBER(0x97)
        SET_SP_MEMBER,
        // SET_RP_MEMBER(0x98)
        SET_RP_MEMBER,
        // GET_ELEMENT(0x99)
        GET_ELEMENT,
        // SET_ELEMENT(0x9a)
        SET_ELEMENT,
        // GET_ARRAY_LEN(0x9b)
        GET_ARRAY_LEN,
        // NEW_ARRAY(0x9c)
        NEW_ARRAY,
        // ARRAY_INSERT(0x9d)
        ARRAY_INSERT,
        // ARRAY_APPEND(0x9e)
        ARRAY_APPEND,
        // ARRAY_ERASE(0x9f)
        ARRAY_ERASE,
        // ARRAY_FIND(0xa0)
        ARRAY_FIND,
        // ARRAY_CLEAR(0xa1)
        ARRAY_CLEAR,
        // WRITE(0xa2)
        WRITE,
        // ARG(0xa3)
        ARG,
        // CALL(0xa4)
        CALL,
        // RETURN(0xa5)
        RETURN,
        // B(0xa6)
        B,
        // BEZ(0xa7)
        BEZ,
        // BNEZ(0xa8)
        BNEZ,
        // CASTsp(0xa9)
        CASTsp,
        // INTb(0xaa)
        INTb,
        // INTc(0xab)
        INTc,
        // INTf(0xac)
        INTf,
        // FLOATb(0xad)
        FLOATb,
        // FLOATc(0xae)
        FLOATc,
        // FLOATi(0xaf)
        FLOATi,
        // BOOLc(0xb0)
        BOOLc,
        // BOOLi(0xb1)
        BOOLi,
        // BOOLf(0xb2)
        BOOLf,
        // GET_OBJ_MEMBER(0xb3)
        GET_OBJ_MEMBER,
        // SET_OBJ_MEMBER(0xb4)
        SET_OBJ_MEMBER,
        // NEW_OBJECT(0xb5)
        NEW_OBJECT,
        // ARRAY_RESIZE(0xb6)
        ARRAY_RESIZE,
        // ARRAY_RESERVE(0xb7)
        ARRAY_RESERVE,
        // LCv4(0xb8)
        LCv4,
        // LC_NULLo(0xb9)
        LC_NULLo,
        // CASTo(0xba)
        CASTo,
        // GET_SP_NATIVE_MEMBER(0xbb)
        GET_SP_NATIVE_MEMBER,
        // LCsa(0xbc)
        LCsa,
        // BIT_ORb(0xbd)
        BIT_ORb,
        // BIT_ANDb(0xbe)
        BIT_ANDb,
        // BIT_XORb(0xbf)
        BIT_XORb,
        // CALLVo(0xc0)
        CALLVo,
        // CALLVsp(0xc1)
        CALLVsp,
        // ASSERT(0xc2)
        ASSERT,
        // LCs64(0xc3)
        LCs64,
        // MOVs64(0xc4)
        MOVs64,
        // ADDs64(0xc5)
        ADDs64,
        // EQs64(0xc6)
        EQs64,
        // NEs64(0xc7)
        NEs64,
        // BIT_ORs64(0xc8)
        BIT_ORs64,
        // BIT_ANDs64(0xc9)
        BIT_ANDs64,
        // BIT_XORs64(0xca)
        BIT_XORs64
    }

    public sealed class InstructionBody
    {
        private static readonly InstructionClass[] INSTRUCTION_CLASSES;

        static InstructionBody()
        {
            var classList = new List<InstructionClass>();
            classList.Add(InstructionClass.NOP);
            classList.AddRange(Enumerable.Repeat(InstructionClass.LOAD_CONST, 6));
            classList.AddRange(Enumerable.Repeat(InstructionClass.UNARY, 20));
            classList.Add(InstructionClass.NOP);
            classList.AddRange(Enumerable.Repeat(InstructionClass.UNARY, 4));
            classList.AddRange(Enumerable.Repeat(InstructionClass.BINARY, 50));
            classList.Add(InstructionClass.NOP);
            classList.Add(InstructionClass.NOP);
            classList.AddRange(Enumerable.Repeat(InstructionClass.BINARY, 6));
            classList.AddRange(Enumerable.Repeat(InstructionClass.GET_BUILTIN_MEMBER, 31));
            classList.AddRange(Enumerable.Repeat(InstructionClass.SET_BUILTIN_MEMBER, 28));
            classList.AddRange(Enumerable.Repeat(InstructionClass.GET_MEMBER, 2));
            classList.AddRange(Enumerable.Repeat(InstructionClass.SET_MEMBER, 2));
            classList.Add(InstructionClass.GET_ELEMENT);
            classList.Add(InstructionClass.SET_ELEMENT);
            classList.Add(InstructionClass.GET_BUILTIN_MEMBER);
            classList.Add(InstructionClass.NEW_ARRAY);
            classList.AddRange(Enumerable.Repeat(InstructionClass.SET_ELEMENT, 3));
            classList.Add(InstructionClass.GET_ELEMENT);
            classList.Add(InstructionClass.SET_ELEMENT);
            classList.Add(InstructionClass.WRITE);
            classList.Add(InstructionClass.ARG);
            classList.Add(InstructionClass.CALL);
            classList.Add(InstructionClass.RETURN);
            classList.AddRange(Enumerable.Repeat(InstructionClass.BRANCH, 3));
            classList.Add(InstructionClass.CAST);
            classList.AddRange(Enumerable.Repeat(InstructionClass.UNARY, 9));
            classList.Add(InstructionClass.GET_MEMBER);
            classList.Add(InstructionClass.SET_MEMBER);
            classList.Add(InstructionClass.NEW_OBJECT);
            classList.AddRange(Enumerable.Repeat(InstructionClass.SET_ELEMENT, 2));
            classList.AddRange(Enumerable.Repeat(InstructionClass.LOAD_CONST, 2));
            classList.Add(InstructionClass.CAST);
            classList.Add(InstructionClass.GET_MEMBER);
            classList.Add(InstructionClass.LOAD_CONST);
            classList.AddRange(Enumerable.Repeat(InstructionClass.BINARY, 3));
            classList.AddRange(Enumerable.Repeat(InstructionClass.CALL, 2));
            classList.Add(InstructionClass.WRITE);
            classList.Add(InstructionClass.LOAD_CONST);
            classList.Add(InstructionClass.UNARY);
            classList.AddRange(Enumerable.Repeat(InstructionClass.BINARY, 6));
            INSTRUCTION_CLASSES = classList.ToArray();
        }
        private readonly InstructionType type;

        public InstructionBody
        (int type)
        {
            this.type = (InstructionType)type;
        }

        public InstructionType getType()
        {
            return type;
        }

        public static InstructionBody
         fromValue(int type)
        {
            if (Enum.IsDefined(typeof(InstructionType), type))
        {
            return new InstructionBody
            (type);
        }
            return default(InstructionBody);
        }        
    }
}