// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("bmtvwUBu/4F/WRkZepioLjpAG4YsVbDPTeETtmGL2F+WTS6Gdx5ie8NcHaK9ZicYsi0xwwfPaC/lgKQJjjtYIRvUbCul64sUSY+Ac5JDaU6rKCYpGasoIyurKCgp3cQtC75E/67tWdFM8lK5wfa8uMbRC1W+Ow6uL2QjZkYuah+gfgY5Mm6F+hl4ti4sdyVJs+N7FS+umBmfpQjsbCfFYG+ILUTd520lO2yj55hpBkVgAjSPI6dxBuKTUbnZ3Ejb9PuYerwC7hxgWhtg5ts84xR8Y6sfcU443FpXKA3IgKCDSTcyxEY+NwwT94crdrRPGasoCxkkLyADr2Gv3iQoKCgsKSrko+A+WeX9eKMQiPsJg/c1wwMy0mlq1Z1jntlDlCsqKCko");
        private static int[] order = new int[] { 6,12,2,13,12,10,9,10,11,10,12,11,12,13,14 };
        private static int key = 41;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
