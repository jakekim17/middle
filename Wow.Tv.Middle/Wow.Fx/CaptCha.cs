using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;

namespace Wow.Fx
{
    public class CaptCha
    {
        public CaptChaResult MakeImage()
        {
            CaptChaResult captChaResult = new CaptChaResult();

            //임의의 글자를 난수로 발생시켜 PrintStr에 집어넣기
            string PrintStr = MakeRandomString();

            //비트맵객체를 생성하고 이 객체를 Graphics객체에서 생성한다.
            Bitmap btm = new Bitmap(100, 80);
            Graphics grp = Graphics.FromImage(btm);
            //회색바탕의 사각형을 만들기
            SolidBrush backBrush = new SolidBrush(Color.DarkGray);
            Rectangle rect = new Rectangle(0, 0, 100, 80);//100,80의 사이즈
            grp.FillRectangle(backBrush, rect);//뒷 배경과 사각형 객체를 전달한다.
                                               //빨간색 글씨를 써서 집어넣는다.
            Font font = new Font("굴림", 20);
            SolidBrush strinBrush = new SolidBrush(Color.Red);
            grp.DrawString(PrintStr, font, strinBrush, 20, 20);

            MemoryStream ms = new MemoryStream();

            btm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            captChaResult.Text = PrintStr;
            captChaResult.Image = ms;

            return captChaResult;
        }


        private string MakeRandomString()
        {
            Random r = new Random();
            //string[] RandomStr = new string[] { "자동", "가입", "프로", "그램", "쓰지", "말자" };
            //string PrintStr = RandomStr[r.Next(6)];
            string PrintStr = r.Next(1000, 9999).ToString();

            return PrintStr;
        }
    }




    public class CaptChaResult
    {
        public MemoryStream Image { get; set; }
        public string Text { get; set; }
    }
}
