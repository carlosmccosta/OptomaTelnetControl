using System;

namespace OptomaTelnetControl.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            OptomaTelnetControl optomaTelnetControl = new OptomaTelnetControl();
            optomaTelnetControl.LoadXmls();

            if (optomaTelnetControl.Connect())
            {
                while (true)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    switch (input.Key)
                    {
                        case ConsoleKey.W:
                        {
                            optomaTelnetControl.IncreaseLensVerticalShift();
                            break;
                        }

                        case ConsoleKey.S:
                        {
                            optomaTelnetControl.DecreaseLensVerticalShift();
                            break;
                        }

                        case ConsoleKey.A:
                        {
                            optomaTelnetControl.DecreaseLensHorizontalShift();
                            break;
                        }

                        case ConsoleKey.D:
                        {
                            optomaTelnetControl.IncreaseLensHorizontalShift();
                            break;
                        }

                        case ConsoleKey.Z:
                        {
                            optomaTelnetControl.IncreaseZoom();
                            break;
                        }

                        case ConsoleKey.X:
                        {
                            optomaTelnetControl.DecreaseZoom();
                            break;
                        }

                        case ConsoleKey.F:
                        {
                            optomaTelnetControl.IncreaseFocus();
                            break;
                        }

                        case ConsoleKey.G:
                        {
                            optomaTelnetControl.DecreaseFocus();
                            break;
                        }

                        case ConsoleKey.NumPad1:
                        {
                            optomaTelnetControl.SaveLensShiftPosition(1);
                            break;
                        }

                        case ConsoleKey.NumPad2:
                        {
                            optomaTelnetControl.SaveLensShiftPosition(2);
                            break;
                        }

                        case ConsoleKey.NumPad3:
                        {
                            optomaTelnetControl.SaveLensShiftPosition(3);
                            break;
                        }

                        case ConsoleKey.NumPad4:
                        {
                            optomaTelnetControl.SaveLensShiftPosition(4);
                            break;
                        }

                        case ConsoleKey.NumPad5:
                        {
                            optomaTelnetControl.SaveLensShiftPosition(5);
                            break;
                        }

                        case ConsoleKey.NumPad6:
                        {
                            optomaTelnetControl.LoadLensShiftPosition(1);
                            break;
                        }

                        case ConsoleKey.NumPad7:
                        {
                            optomaTelnetControl.LoadLensShiftPosition(2);
                            break;
                        }

                        case ConsoleKey.NumPad8:
                        {
                            optomaTelnetControl.LoadLensShiftPosition(3);
                            break;
                        }

                        case ConsoleKey.NumPad9:
                        {
                            optomaTelnetControl.LoadLensShiftPosition(4);
                            break;
                        }

                        case ConsoleKey.NumPad0:
                        {
                            optomaTelnetControl.LoadLensShiftPosition(5);
                            break;
                        }

                        case ConsoleKey.N:
                        {
                            optomaTelnetControl.PowerOn();
                            break;
                        }

                        case ConsoleKey.M:
                        {
                            optomaTelnetControl.PowerOff();
                            break;
                        }

                        case ConsoleKey.L:
                        {
                            optomaTelnetControl.LockLens(true);
                            break;
                        }

                        case ConsoleKey.K:
                        {
                            optomaTelnetControl.LockLens(false);
                            break;
                        }

                        case ConsoleKey.H:
                        {
                            optomaTelnetControl.CenterLensShift();
                            break;
                        }
                    }
                }
            }

            optomaTelnetControl.SaveXmls();
        }
    }
}
