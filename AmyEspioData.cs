using System.Runtime.InteropServices;

namespace sonicheroes.amyespio.abilities
{
    [StructLayout(LayoutKind.Sequential)]
    public class AmyEspioData
    {
        public byte region1_0;
        public byte region1_1;
        public byte region1_2;
        public byte region1_3;
        public byte region1_4;
        public byte region1_5;
        public byte region1_6;
        public byte region1_7;
        public byte region1_8;

        public AmyEspioData(byte region1_0, byte region1_1, byte region1_2, byte region1_3, byte region1_4, byte region1_5, byte region1_6, byte region1_7, byte region1_8)
        {
            this.region1_0 = region1_0;
            this.region1_1 = region1_1;
            this.region1_2 = region1_2;
            this.region1_3 = region1_3;
            this.region1_4 = region1_4;
            this.region1_5 = region1_5;
            this.region1_6 = region1_6;
            this.region1_7 = region1_7;
            this.region1_8 = region1_8;
        }
    }
}
