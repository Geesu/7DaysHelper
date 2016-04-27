using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _7daysHelper
{
    class ChatHook
    {
        private const string BBFILTER = "[ffffffff][/url][/b][/i][/u][/s][/sub][/sup][ff]";

        // converts a time to the appropriate day, hour and minute
        private static void TimeToDayTime(ulong time, out int day, out int hour, out int minute)
        {
            day = (int)Math.Floor((float)time / 24000f) - 1;

            if ( day < 0 )
            {
                day = 0;
            }

            ulong remainder = time % 24000;

            hour = (((int)(time / ((ulong)1000))) % 24);

            remainder = remainder % 1000;

            minute = (((int)((((double)time) / 1000.0) * 60.0)) % 60);
        }

        public static bool Hook(ClientInfo _cInfo, string _message, string _playerName)
        {
            if (!string.IsNullOrEmpty(_message))
            {
                // player message
                if (_message.EndsWith(BBFILTER + BBFILTER))
                {
                    _message = _message.Remove(_message.Length - 2 * BBFILTER.Length);
                }

                // server message
                if (_message.EndsWith(BBFILTER))
                {
                    _message = _message.Remove(_message.Length - BBFILTER.Length);
                }

                if (_message.ToLower() == "/7days")
                {
                    int currentDay = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
                    int currentHour = GameUtils.WorldTimeToHours(GameManager.Instance.World.worldTime);
                    int currentMinute = GameUtils.WorldTimeToMinutes(GameManager.Instance.World.worldTime);

                    int dayLength = GameStats.GetInt(EnumGameStats.DayLightLength);

                    // determine if we are within the horde period for day 7
                    Boolean IsInDay7 = false;
                    if (currentDay >= 7)
                    {
                        if (currentDay % 7 == 0 && currentHour >= 22)
                        {
                            IsInDay7 = true;
                        }
                        // day 8 before 4 AM (assuming default day length of 18)
                        else if (currentDay % 8 == 0 && currentHour < 24 - dayLength - 2)
                        {
                            IsInDay7 = true;
                        }
                    }

                    // not in day 7 horde period
                    if (!IsInDay7)
                    {
                        // find the next day 7, duh.
                        int daysUntilHorde = 7 - (currentDay % 7);

                        // when the next horde spawns
                        ulong nextHordeTime = GameUtils.DayTimeToWorldTime(currentDay + daysUntilHorde, 22, 0);

                        ulong timeUntilHorde = nextHordeTime - GameManager.Instance.World.worldTime;

                        int hourUntilHorde = GameUtils.WorldTimeToHours(timeUntilHorde);
                        int minutesUntilHorde = GameUtils.WorldTimeToMinutes(timeUntilHorde);

                        // green
                        String color = "00ba67";
                        if ( daysUntilHorde < 3)
                        {
                            // red
                            color = "ff0000";
                        }
                        else if ( daysUntilHorde < 5 )
                        {
                            // yellow
                            color = "fafc57";
                        }

                        String response = String.Format("[{0}]Next horde in {1} days, {2} hours, {3} minutes. Prepare![-]", color, daysUntilHorde, hourUntilHorde, minutesUntilHorde);
                        if (_cInfo != null)
                        { 
                            _cInfo.SendPackage(new NetPackageGameMessage(response, ""));
                        }
                        else
                        {
                            Log.Out(response);
                        }
                    }
                    else
                    {
                        String response = String.Format("[ff0000]Day 7 Horde is now![-]");
                        if (_cInfo != null)
                        {
                            _cInfo.SendPackage(new NetPackageGameMessage(response, ""));
                        }
                        else
                        {
                            Log.Out(response);
                        }
                    }
                }
            }

            return true;
        }
    }
}
