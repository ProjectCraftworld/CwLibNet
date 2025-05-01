namespace CwLibNet.Enums;

public enum InstructionType : byte
{
    // NOP(0x0)
    NOP,
    // LCb(0x1)
    L_CB,
    // LCc(0x2)
    L_CC,
    // LCi(0x3)
    L_CI,
    // LCf(0x4)
    L_CF,
    // LCsw(0x5)
    L_CSW,
    // LC_NULLsp(0x6)
    LC_NUL_LSP,
    // MOVb(0x7)
    MO_VB,
    // LOG_NEGb(0x8)
    LOG_NE_GB,
    // MOVc(0x9)
    MO_VC,
    // MOVi(0xa)
    MO_VI,
    // INCi(0xb)
    IN_CI,
    // DECi(0xc)
    DE_CI,
    // NEGi(0xd)
    NE_GI,
    // BIT_NEGi(0xe)
    BIT_NE_GI,
    // LOG_NEGi(0xf)
    LOG_NE_GI,
    // ABSi(0x10)
    AB_SI,
    // MOVf(0x11)
    MO_VF,
    // NEGf(0x12)
    NE_GF,
    // ABSf(0x13)
    AB_SF,
    // SQRTf(0x14)
    SQR_TF,
    // SINf(0x15)
    SI_NF,
    // COSf(0x16)
    CO_SF,
    // TANf(0x17)
    TA_NF,
    // MOVv4(0x18)
    MO_VV4,
    // NEGv4(0x19)
    NE_GV4,
    // MOVm44(0x1a)
    MO_VM44,
    // @Deprecated
    // MOVs(0x1b)
    MO_VS,
    // MOVrp(0x1c)
    MO_VRP,
    // MOVcp(0x1d)
    MO_VCP,
    // MOVsp(0x1e)
    MO_VSP,
    // MOVo(0x1f)
    MO_VO,
    // EQb(0x20)
    E_QB,
    // NEb(0x21)
    N_EB,
    // RESERVED0(0x22)
    RESERVED0,
    // RESERVED1(0x23)
    RESERVED1,
    // LTc(0x24)
    L_TC,
    // LTEc(0x25)
    LT_EC,
    // GTc(0x26)
    G_TC,
    // GTEc(0x27)
    GT_EC,
    // EQc(0x28)
    E_QC,
    // NEc(0x29)
    N_EC,
    // ADDi(0x2a)
    AD_DI,
    // SUBi(0x2b)
    SU_BI,
    // MULi(0x2c)
    MU_LI,
    // DIVi(0x2d)
    DI_VI,
    // MODi(0x2e)
    MO_DI,
    // MINi(0x2f)
    MI_NI,
    // MAXi(0x30)
    MA_XI,
    // SLAi(0x31)
    SL_AI,
    // SRAi(0x32)
    SR_AI,
    // SRLi(0x33)
    SR_LI,
    // BIT_ORi(0x34)
    BIT_O_RI,
    // BIT_ANDi(0x35)
    BIT_AN_DI,
    // BIT_XORi(0x36)
    BIT_XO_RI,
    // LTi(0x37)
    L_TI,
    // LTEi(0x38)
    LT_EI,
    // GTi(0x39)
    G_TI,
    // GTEi(0x3a)
    GT_EI,
    // EQi(0x3b)
    E_QI,
    // NEi(0x3c)
    N_EI,
    // ADDf(0x3d)
    AD_DF,
    // SUBf(0x3e)
    SU_BF,
    // MULf(0x3f)
    MU_LF,
    // DIVf(0x40)
    DI_VF,
    // MINf(0x41)
    MI_NF,
    // MAXf(0x42)
    MA_XF,
    // LTf(0x43)
    L_TF,
    // LTEf(0x44)
    LT_EF,
    // GTf(0x45)
    G_TF,
    // GTEf(0x46)
    GT_EF,
    // EQf(0x47)
    E_QF,
    // NEf(0x48)
    N_EF,
    // ADDv4(0x49)
    AD_DV4,
    // SUBv4(0x4a)
    SU_BV4,
    // MULSv4(0x4b)
    MUL_SV4,
    // DIVSv4(0x4c)
    DIV_SV4,
    // DOT4v4(0x4d)
    DOT4_V4,
    // DOT3v4(0x4e)
    DOT3_V4,
    // DOT2v4(0x4f)
    DOT2_V4,
    // CROSS3v4(0x50)
    CROSS3_V4,
    // MULm44(0x51)
    MU_LM44,
    // @Deprecated
    // EQs(0x52)
    E_QS,
    // @Deprecated
    // NEs(0x53)
    N_ES,
    // EQrp(0x54)
    E_QRP,
    // NErp(0x55)
    N_ERP,
    // EQo(0x56)
    E_QO,
    // NEo(0x57)
    N_EO,
    // EQsp(0x58)
    E_QSP,
    // NEsp(0x59)
    N_ESP,
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
    GET_M44_R_X,
    // GET_M44_rY(0x72)
    GET_M44_R_Y,
    // GET_M44_rZ(0x73)
    GET_M44_R_Z,
    // GET_M44_rW(0x74)
    GET_M44_R_W,
    // GET_M44_cX(0x75)
    GET_M44_C_X,
    // GET_M44_cY(0x76)
    GET_M44_C_Y,
    // GET_M44_cZ(0x77)
    GET_M44_C_Z,
    // GET_M44_cW(0x78)
    GET_M44_C_W,
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
    SET_M44_R_X,
    // SET_M44_rY(0x8e)
    SET_M44_R_Y,
    // SET_M44_rZ(0x8f)
    SET_M44_R_Z,
    // SET_M44_rW(0x90)
    SET_M44_R_W,
    // SET_M44_cX(0x91)
    SET_M44_C_X,
    // SET_M44_cY(0x92)
    SET_M44_C_Y,
    // SET_M44_cZ(0x93)
    SET_M44_C_Z,
    // SET_M44_cW(0x94)
    SET_M44_C_W,
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
    CAS_TSP,
    // INTb(0xaa)
    IN_TB,
    // INTc(0xab)
    IN_TC,
    // INTf(0xac)
    IN_TF,
    // FLOATb(0xad)
    FLOA_TB,
    // FLOATc(0xae)
    FLOA_TC,
    // FLOATi(0xaf)
    FLOA_TI,
    // BOOLc(0xb0)
    BOO_LC,
    // BOOLi(0xb1)
    BOO_LI,
    // BOOLf(0xb2)
    BOO_LF,
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
    L_CV4,
    // LC_NULLo(0xb9)
    LC_NUL_LO,
    // CASTo(0xba)
    CAS_TO,
    // GET_SP_NATIVE_MEMBER(0xbb)
    GET_SP_NATIVE_MEMBER,
    // LCsa(0xbc)
    L_CSA,
    // BIT_ORb(0xbd)
    BIT_O_RB,
    // BIT_ANDb(0xbe)
    BIT_AN_DB,
    // BIT_XORb(0xbf)
    BIT_XO_RB,
    // CALLVo(0xc0)
    CALL_VO,
    // CALLVsp(0xc1)
    CALL_VSP,
    // ASSERT(0xc2)
    ASSERT,
    // LCs64(0xc3)
    L_CS64,
    // MOVs64(0xc4)
    MO_VS64,
    // ADDs64(0xc5)
    AD_DS64,
    // EQs64(0xc6)
    E_QS64,
    // NEs64(0xc7)
    N_ES64,
    // BIT_ORs64(0xc8)
    BIT_O_RS64,
    // BIT_ANDs64(0xc9)
    BIT_AN_DS64,
    // BIT_XORs64(0xca)
    BIT_XO_RS64
}

public sealed class InstructionBody(int type)
{
    private static readonly InstructionClass[] InstructionClasses;

    static InstructionBody()
    {
        var classList = new List<InstructionClass> { InstructionClass.NOP };
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
        InstructionClasses = classList.ToArray();
    }
    private readonly InstructionType type = (InstructionType)type;

    public static InstructionBody
        FromValue(int type)
    {
        if (Enum.IsDefined(typeof(InstructionType), type))
        {
            return new InstructionBody
                (type);
        }
        return null;
    }        
}