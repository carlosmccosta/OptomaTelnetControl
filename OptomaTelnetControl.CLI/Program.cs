using System;

namespace OptomaTelnetControl.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            OptomaControl optomaControl = new OptomaControl();

            //optomaControl.Profiles.ProfilesList.Add(new State());
            //optomaControl.Profiles.ProfilesList.Add(new State());
            //optomaControl.SaveXmls();
            //return;

            optomaControl.LoadXmls();

            int commandRepeatTimes = 1;

            if (optomaControl.Connect())
            {
                bool stop = false;
                while (!stop)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    switch (input.Key)
                    {
                        case ConsoleKey.W:
                        {
                            optomaControl.MoveLensVerticalShiftUp(commandRepeatTimes);
                            break;
                        }

                        case ConsoleKey.S:
                        {
                            optomaControl.MoveLensVerticalShiftDown(commandRepeatTimes);
                            break;
                        }

                        case ConsoleKey.A:
                        {
                            optomaControl.MoveLensHorizontalShiftLeft(commandRepeatTimes);
                            break;
                        }

                        case ConsoleKey.D:
                        {
                            optomaControl.MoveLensHorizontalShiftRight(commandRepeatTimes);
                            break;
                        }

                        case ConsoleKey.Z:
                        {
                            optomaControl.IncreaseZoomForHigherProjectionSize(commandRepeatTimes);
                            break;
                        }

                        case ConsoleKey.X:
                        {
                            optomaControl.DecreaseZoomForLowerProjectionSize(commandRepeatTimes);
                            break;
                        }

                        case ConsoleKey.F:
                        {
                            optomaControl.IncreaseFocusToHigherProjectionDistance(commandRepeatTimes);
                            break;
                        }

                        case ConsoleKey.G:
                        {
                            optomaControl.DecreaseFocusToLowerProjectionDistance(commandRepeatTimes);
                            break;
                        }

                        case ConsoleKey.NumPad1:
                        {
                            optomaControl.SaveLensShiftPosition(1);
                            break;
                        }

                        case ConsoleKey.NumPad2:
                        {
                            optomaControl.SaveLensShiftPosition(2);
                            break;
                        }

                        case ConsoleKey.NumPad3:
                        {
                            optomaControl.SaveLensShiftPosition(3);
                            break;
                        }

                        case ConsoleKey.NumPad4:
                        {
                            optomaControl.SaveLensShiftPosition(4);
                            break;
                        }

                        case ConsoleKey.NumPad5:
                        {
                            optomaControl.SaveLensShiftPosition(5);
                            break;
                        }

                        case ConsoleKey.NumPad6:
                        {
                            optomaControl.ApplyLensShiftPosition(1);
                            break;
                        }

                        case ConsoleKey.NumPad7:
                        {
                            optomaControl.ApplyLensShiftPosition(2);
                            break;
                        }

                        case ConsoleKey.NumPad8:
                        {
                            optomaControl.ApplyLensShiftPosition(3);
                            break;
                        }

                        case ConsoleKey.NumPad9:
                        {
                            optomaControl.ApplyLensShiftPosition(4);
                            break;
                        }

                        case ConsoleKey.NumPad0:
                        {
                            optomaControl.ApplyLensShiftPosition(5);
                            break;
                        }

                        case ConsoleKey.T:
                        {
                            optomaControl.ActivateStoredProfile();
                            break;
                        }

                        case ConsoleKey.N:
                        {
                            optomaControl.PowerOn();
                            break;
                        }

                        case ConsoleKey.M:
                        {
                            optomaControl.PowerOff();
                            break;
                        }

                        case ConsoleKey.L:
                        {
                            optomaControl.LockLens(true);
                            break;
                        }

                        case ConsoleKey.K:
                        {
                            optomaControl.LockLens(false);
                            break;
                        }

                        case ConsoleKey.H:
                        {
                            optomaControl.RecalibrateLensShiftToCenter();
                            break;
                        }

                        case ConsoleKey.Y:
                        {
                            optomaControl.ResetHorizontalLensShiftToLeft();
                            break;
                        }

                        case ConsoleKey.U:
                        {
                            optomaControl.ResetVerticalLensShiftToBottom();
                            break;
                        }

                        case ConsoleKey.I:
                        {
                            optomaControl.ResetFocusToClosestProjectionDistance();
                            break;
                        }

                        case ConsoleKey.O:
                        {
                            optomaControl.ResetZoomToSmallestProjectionSize();
                            break;
                        }

                        case ConsoleKey.Backspace:
                        {
                            if (commandRepeatTimes > 1)
                            {
                                commandRepeatTimes -= 1;
                            }
                            break;
                        }

                        case ConsoleKey.Enter:
                        {
                            commandRepeatTimes += 1;
                            break;
                        }

                        case ConsoleKey.Delete:
                        {
                            commandRepeatTimes = 1;
                            break;
                        }

                        case ConsoleKey.Escape:
                        {
                            stop = true;
                            break;
                        }
                    }
                }
            }

            optomaControl.SaveXmls();
            optomaControl.Dispose();
        }
    }
}
