using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows;

namespace Project1
{
    class Shooting
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetKeyState(int key);
        private const int KeyPressed = 0x8000;
        private const int VK_LEFT = 0x25;
        private const int VK_UP = 0X26;
        private const int VK_RIGHT = 0X27;
        private const int VK_DOWN = 0X28;
        private const int VK_SPACE = 0X20;
        private const int VK_C = 0x43;

        Random rand = new Random();

        public int GetHp()
        {
            return nHP;
        }

        public int GetWidth()
        {
            return nWidth;
        }
        private static Mutex mutex = new Mutex();

        Thread workerA;
        Thread workerB;

        int needScore = 10;

        bool keyPress;
        bool IsnPostion = false;
        int HpDe = 30;
        int stage = 1;
        int Positonsecond = 30;
        int nPostionDelay = 10;
        int nPostion = 2;
        int nHP = 3;
        int nScore = 0;
        int nHpstion = 3;
        int UserX = 25, UserY = 27;
        int nHegiht = 29;
        int nWidth = 50;
        int nBulletMax = 0;

        char chA = 'b';
        bool HpUse = true;

        int[] ShootX = new int[20];
        int[] ShootY = new int[20];
        bool[] bullet = new bool[20];

        int[] EnermyX = new int[10];
        int[] EnermyY = new int[10];
        bool[] IsEnermy = new bool[10];
        bool[] IsEnermySpawn = new bool[10];

        int EnernmyBulltCount = 3;
        int[] EnermyBulltX = new int[3];
        int[] EnermyBulltY = new int[3];
        bool[] IsEnermyBullt = new bool[3];
        bool[] IsEnermyBulltSpawn = new bool[3];
        int EnermyBulltDelay = 13;
        int EnermyBulltDelay2 = 6;

        int Delay = 10;

        ConsoleKeyInfo input;

        public void HpDealy() // 포션키누르면 계속먹는거 방지
        {
            if (!HpUse && HpDe < 0)
            {
                HpDe = 30;
                HpUse = true;
            }
        }

        public void _nPostion()
        {
            Positonsecond--;
            if (Positonsecond <= 0)
            {
                nPostionDelay--;
                Positonsecond = 30;
            }
        }

        public void SetStage()
        {
            nBulletMax = 0;
            for (int i = 0; i < 6; i++)
            {
                bullet[i] = false;
            }

            for (int i = 0; i < 10; i++)
            {
                IsEnermy[i] = false;
                IsEnermySpawn[i] = false;
            }

            for (int i = 0; i < EnernmyBulltCount; i++)
            {
                IsEnermyBullt[i] = false;
                IsEnermyBulltSpawn[i] = false;
            }
        }


        public void SetEnermyBullt()
        {
            for (int i = 0; i < EnernmyBulltCount; i++)
            {
                if (!IsEnermyBullt[i])
                {
                    int ran = rand.Next(1, nWidth - 1);
                    EnermyBulltX[i] = ran;
                    EnermyBulltY[i] = 3;
                    IsEnermyBullt[i] = true;
                }
            }
        }

        public void Move()
        {

            Console.SetCursorPosition(UserX, UserY);
            Console.Write("  ");
            Console.SetCursorPosition(0, 0);
            keyPress = true;

            if ((GetKeyState(VK_LEFT) & KeyPressed) != 0)
            {
                UserX--;
                PrintUser();
            }
            else
            {
                PrintUser();
            }

            if ((GetKeyState(VK_RIGHT) & KeyPressed) != 0)
            {
                UserX++;
                Console.SetCursorPosition(UserX - 1, UserY);
                Console.Write("   ");
                PrintUser();
            }
            else
            {
                PrintUser();
            }

            if ((GetKeyState(VK_UP) & KeyPressed) != 0)
            {
                UserY--;
                Console.SetCursorPosition(UserX, UserY + 1);
                Console.Write("  ");
                PrintUser();
            }
            else
            {
                PrintUser();
            }
            if ((GetKeyState(VK_DOWN) & KeyPressed) != 0)
            {
                UserY++;
                Console.SetCursorPosition(UserX, UserY - 1);
                Console.Write("  ");
                Console.SetCursorPosition(UserX, UserY);
                Console.Write("▲");
            }
            else
            {
                PrintUser();
            }


            if ((GetKeyState(VK_SPACE) & KeyPressed) != 0)
            {
                if (nBulletMax < 6)
                {
                    nBulletMax++;
                    for (int i = 0; i < 6; i++)
                    {
                        if (!bullet[i])
                        {
                            ShootX[i] = UserX;
                            ShootY[i] = UserY - 1;
                            bullet[i] = true;
                            break;
                        }
                    }
                }
            }


            if ((GetKeyState(VK_C) & KeyPressed) != 0)
            {
                if (HpUse)
                {
                    if (nHpstion > 0)
                    {
                        HpUse = false;
                        nHP = 3;
                        nHpstion -= 1;
                        Thread.Sleep(10);
                    }
                }
            }
            else // 키를떼야 트루가됨. 포션키누르면 계속먹는거 방지
            {
                HpDealy();
            }

            if (UserX > nWidth - 2)
            {
                UserX = nWidth - 2;
            }
            if (UserX < 2)
            {
                Console.SetCursorPosition(UserX, UserY);
                Console.Write(" ");
                UserX = 2;
            }
            if (UserY > nHegiht - 1)
            {
                UserY = nHegiht - 1;
            }
            if (UserY < 2)
            {
                UserY = 3;
            }

            for (int i = 0; i < 6; i++)
            {
                if (bullet[i])
                {
                    Console.SetCursorPosition(ShootX[i], ShootY[i]);
                    Console.Write("  ");
                    Console.SetCursorPosition(ShootX[i], ShootY[i] - 1);
                    Console.Write("*");
                    ShootY[i]--;
                    if (ShootY[i] < 2)
                    {
                        Console.SetCursorPosition(ShootX[i], ShootY[i]);
                        Console.Write(" ");
                        nBulletMax--;
                        bullet[i] = false;
                        ShootX[i] = 0;
                        ShootY[i] = 0;
                    }
                }
            }

            Console.SetCursorPosition(0, 0);
        }

        public void Enermy()
        {
            Delay--;
            EnermyBulltDelay--;

            SetEnemy();
            SetEnermyBullt();

            if (Delay == 0)
            {
                Delay = 7;
                for (int i = 0; i < 10; i++)
                {
                    if (IsEnermy[i] && !IsEnermySpawn[i])
                    {
                        IsEnermySpawn[i] = true;
                        break;
                    }
                }
            }

            if (EnermyBulltDelay == 0)
            {
                EnermyBulltDelay = EnermyBulltDelay2;
                for (int i = 0; i < EnernmyBulltCount; i++)
                {
                    if (IsEnermyBullt[i] && !IsEnermyBulltSpawn[i])
                    {
                        IsEnermyBulltSpawn[i] = true;
                        break;
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                if (IsEnermySpawn[i])
                {
                    EnermyY[i]++;
                    if (EnermyY[i] > 28)
                    {
                        Console.SetCursorPosition(EnermyX[i], EnermyY[i] - 1);
                        Console.Write("  ");
                        IsEnermy[i] = false;
                        IsEnermySpawn[i] = false;
                    }

                    if (!IsnPostion)
                    {
                        if (EnermyY[i] == UserY && EnermyX[i] == UserX)
                        {
                            Console.SetCursorPosition(UserX, UserY - 1);
                            Console.Write("  ");
                            IsEnermy[i] = false;
                            IsEnermySpawn[i] = false;
                            nHP -= 1;
                        }
                        else if (EnermyY[i] == UserY && EnermyX[i] == UserX + 1)
                        {
                            Console.SetCursorPosition(UserX + 1, UserY - 1);
                            Console.Write("  ");
                            IsEnermy[i] = false;
                            IsEnermySpawn[i] = false;
                            nHP -= 1;
                        }
                        else if (EnermyY[i] == UserY && EnermyX[i] == UserX - 1)
                        {
                            Console.SetCursorPosition(UserX - 1, UserY - 1);
                            Console.Write("  ");
                            IsEnermy[i] = false;
                            IsEnermySpawn[i] = false;
                            nHP -= 1;
                        }
                    }

                    Console.SetCursorPosition(EnermyX[i], EnermyY[i] - 1);
                    Console.Write("  ");
                    if (IsEnermy[i])
                    {
                        Console.SetCursorPosition(EnermyX[i], EnermyY[i]);
                        Console.Write("⊙");
                    }
                    else
                    {
                        Console.SetCursorPosition(EnermyX[i], EnermyY[i]);
                        Console.Write("   ");
                    }
                }
            }

            for (int i = 0; i < EnernmyBulltCount; i++)
            {
                if (IsEnermyBulltSpawn[i])
                {
                    Console.SetCursorPosition(EnermyBulltX[i], EnermyBulltY[i] - 1);
                    Console.Write("  ");
                    Console.SetCursorPosition(EnermyBulltX[i], EnermyBulltY[i]);
                    Console.Write("↓");
                    EnermyBulltY[i]++;
                    if (EnermyBulltY[i] > 27)
                    {
                        Console.SetCursorPosition(EnermyBulltX[i], EnermyBulltY[i] - 1);
                        Console.Write("  ");
                        IsEnermyBullt[i] = false;
                        IsEnermyBulltSpawn[i] = false;
                    }

                    if (!IsnPostion)
                    {
                        if (EnermyBulltY[i] == UserY && EnermyBulltX[i] == UserX)
                        {
                            Console.SetCursorPosition(UserX, UserY - 1);
                            Console.Write("  ");
                            IsEnermyBullt[i] = false;
                            IsEnermyBulltSpawn[i] = false;
                            nHP -= 1;
                            break;
                        }
                        else if (EnermyBulltY[i] == UserY && EnermyBulltX[i] == UserX + 1)
                        {
                            Console.SetCursorPosition(UserX + 1, UserY - 1);
                            Console.Write("  ");
                            IsEnermyBullt[i] = false;
                            IsEnermyBulltSpawn[i] = false;
                            nHP -= 1;
                            break;
                        }
                        else if (EnermyBulltY[i] == UserY && EnermyBulltX[i] == UserX - 1)
                        {
                            Console.SetCursorPosition(UserX - 1, UserY - 1);
                            Console.Write("  ");
                            IsEnermyBullt[i] = false;
                            IsEnermyBulltSpawn[i] = false;
                            nHP -= 1;
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (bullet[i])
                    {
                        if (EnermyX[j] == ShootX[i] && EnermyY[j] == ShootY[i])
                        {
                            Console.SetCursorPosition(EnermyX[i], EnermyX[i]);
                            Console.Write("  ");
                            Console.SetCursorPosition(ShootX[i], ShootY[i]);
                            Console.Write("  ");
                            Console.SetCursorPosition(ShootX[i], ShootY[i] - 1);
                            Console.Write("  ");
                            EnermyHit(j);
                            nBulletMax--;
                            bullet[i] = false;
                        }
                        else if (EnermyX[j] >= ShootX[i] - 1 && EnermyX[j] <= ShootX[i] + 1 &&
                                 EnermyY[j] >= ShootY[i] - 1 && EnermyY[j] < ShootY[i] + 1)
                        {
                            Console.SetCursorPosition(EnermyX[j], ShootY[i] - 1);
                            Console.Write("  ");
                            Console.SetCursorPosition(EnermyX[j] - 1, ShootY[i] - 1);
                            Console.Write("  ");
                            Console.SetCursorPosition(EnermyX[j] + 1, ShootY[i] - 1);
                            Console.Write("  ");
                            Console.SetCursorPosition(EnermyX[j] - 1, ShootY[i]);
                            Console.Write("  ");
                            Console.SetCursorPosition(EnermyX[j] + 1, ShootY[i]);
                            Console.Write("  ");
                            EnermyHit(j);
                            nBulletMax--;
                            bullet[i] = false;
                        }
                    }
                }
            }
            Console.SetCursorPosition(0, 0);

        }

        public void EnermyHit(int _index)
        {
            IsEnermy[_index] = false;
            IsEnermySpawn[_index] = false;
            nScore += 1;
        }

        public void SetEnemy()
        {
            for (int i = 0; i < 10; i++)
            {
                if (!IsEnermy[i])
                {
                    int ran = rand.Next(2, nWidth - 1);
                    EnermyX[i] = ran;
                    EnermyY[i] = 3;
                    IsEnermy[i] = true;
                }
            }
        }

        public void BackGround()
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 1; i < nHegiht; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write('│');
            }

            Console.SetCursorPosition(0, nHegiht);
            Console.Write('└');

            for (int i = 1; i < nWidth; i++)
            {
                Console.SetCursorPosition(i, nHegiht);
                Console.Write('─');
            }

            for (int i = 1; i < nHegiht; i++)
            {
                Console.SetCursorPosition(nWidth, i);
                Console.Write("│");
            }

            Console.SetCursorPosition(nWidth, nHegiht);
            Console.Write('┘');

            Console.SetCursorPosition(nWidth + 5, 1);
            switch (nHP)
            {
                case 3:
                    Console.Write("체력: ♥♥♥");
                    break;
                case 2:
                    Console.Write("체력: ♥♥♡");
                    break;
                case 1:
                    Console.Write("체력: ♥♡♡");
                    break;
                default:
                    Console.Write("체력: ♡♡♡");
                    break;
            }

            Console.SetCursorPosition(nWidth + 5, 3);
            Console.Write("이동키:화살표 ");

            Console.SetCursorPosition(nWidth + 5, 5);
            Console.Write("공격: SPACE");

            Console.SetCursorPosition(nWidth + 5, 7);
            Console.Write("물약 C키 : " + nHpstion);

            Console.SetCursorPosition(nWidth + 5, 11);
            Console.Write("점수:{0}  ", nScore);

            Console.SetCursorPosition(nWidth + 5, 13);
            Console.Write("필요점수:{0}  ", needScore);

            Console.SetCursorPosition(nWidth + 5, 15);
            Console.Write("stage : " + stage);

            Console.SetCursorPosition(0, 0);
            Console.Write(" ");

            if (nScore >= needScore)
            {
                nScore = needScore;
                needScore += nScore;
                nHP = 3;
                nHpstion = 3;
                Console.Clear();
                System.Threading.Thread.Sleep(500);
                BackGround();
                Console.SetCursorPosition(nWidth + 5, 16);
                Console.Write("스테이지{0}: 클리어 ", stage);
                System.Threading.Thread.Sleep(500);
                if (stage < 3)
                {
                    Console.SetCursorPosition(nWidth + 5, 17);
                    Console.Write("가로길이 감소 -7");
                }
                else
                {
                    Console.SetCursorPosition(nWidth + 5, 18);
                    Console.Write("↓갯수증가 딜레이-1");
                    EnernmyBulltCount++;
                    EnermyBulltDelay2--;
                    EnermyBulltX = new int[EnernmyBulltCount];
                    EnermyBulltY = new int[EnernmyBulltCount];
                    IsEnermyBullt = new bool[EnernmyBulltCount];
                    IsEnermyBulltSpawn = new bool[EnernmyBulltCount];
                }
                Delay = 10;
                EnermyBulltDelay = 13;
                SetStage();
                System.Threading.Thread.Sleep(2000);
                Console.ReadKey();
                Console.Clear();

                if (stage < 3)
                {
                    nWidth -= 10;
                }
                stage++;
            }


        }

        public void Playing()
        {
            HpDe--;// 포션키누르면 계속먹는거 방지
            keyPress = false;
            BackGround();

            if (IsnPostion)
            {
                _nPostion();
            }

            if (keyPress == false)
            {
                PrintUser();
            }
            System.Threading.Thread.Sleep(26);
        }

        public void PrintUser()
        {
            char User = '▲';
            Console.SetCursorPosition(UserX, UserY);
            Console.Write(User);
        }

        static void Main(string[] args)
        {
            Console.SetWindowSize(100, 30);

            Shooting shooting = new Shooting();
            Console.CursorVisible = false;

            shooting.BackGround();

            int hp = shooting.GetHp();
            while (true)
            {

                shooting.Playing();
                shooting.Enermy();
                shooting.Move();
                hp = shooting.GetHp();
                if (hp <= 0)
                {
                    shooting.BackGround();
                    Thread.Sleep(1000);
                    shooting.BackGround();
                    Console.SetCursorPosition(shooting.GetWidth() + 5, 17);
                    Console.Write("게임오버");
                    Console.ReadKey();
                    break;
                }
            }

        }
    }
}
