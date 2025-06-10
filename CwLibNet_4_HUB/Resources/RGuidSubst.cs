using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.Util;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Resources;

public class RGuidSubst: Dictionary<GUID, GUID>
{

    public static RGuidSubst LBP2_TO_LBP1_GMATS = FromCSV(
        "g106493, g106497\ng106492, g106496\ng154680, g38924\ng78865, g87096\ng77356, g66636\ng12719, g1124\ng92128, g66675\ng107287, g75897\ng3842, g2191\ng72428, g72883\ng5207, g16596\ng5190, g16601\ng125425, g8435\ng8436, g8468\ng3840, g16663\ng3843, g16664\ng3905, g16665\ng3841, g16666\ng3848, g16667\ng80432, g64840\ng80891, g81245\ng9493, g11669\ng158530, g9737\ng121620, g8742\ng105961, g5680\ng66129, g66127\ng81029, g67231\ng154680, g68259\ng3848, g2754\ng3850, g2759\ng81633, g81629\ng81634, g81631\ng77356, g26400\ng106491, g106495");

    public static RGuidSubst LBP2_TO_LBP1_PLANS = FromCSV(
        "g83655, g31701\ng83656, g31703\ng83657, g31704\ng83658, g31706\ng83659, g31707\ng83660, g31710\ng83661, g31714\ng83662, g31715\ng83663, g31716\ng83664, g31717\ng83665, g31719\ng83694, g32768\ng83695, g32769\ng83696, g32770\ng83697, g32771\ng83667, g32799\ng83668, g32800\ng83669, g32801\ng83670, g32802\ng83671, g32803\ng83682, g33099\ng83683, g33100\ng83684, g33101\ng83685, g33103\ng83673, g33132\ng83674, g33133\ng83675, g33134\ng83676, g33135\ng83677, g33136\ng83679, g33137\ng83680, g33138\ng83681, g33139\ng83687, g33188\ng83688, g33189\ng83689, g33190\ng83690, g33191\ng83691, g33192\ng83692, g33193\ng83693, g33196\ng83678, g34541\ng83672, g34557\ng156367, g39747\ng83666, g45765\ng130012, g51309\ng156368, g52018\ng103727, g52020\ng124479, g52021\ng103723, g52030\ng103722, g52033\ng92667, g54695\ng83686, g56414\ng83666, g119449");
    
    public static RGuidSubst FromCSV(String data)
    {
        var subst = new RGuidSubst();
        String[] lines = data.Split("\n");
        foreach (var line in lines)
        {
            var line2 = line.Replace("\\s", "");
            if (line2 == "" || line2.StartsWith("#")) continue;

            String[] row = line2.Split(",");
            subst.Add(Strings.GetGUID(row[0])!.Value, Strings.GetGUID(row[1])!.Value);
        }
        return subst;
    }
}