using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace _7daysHelper
{

    public class API : ModApiAbstract
    {
        public override bool ChatMessage(ClientInfo _cInfo, EnumGameMessages _type, string _msg, string _mainName, bool _localizeMain, string _secondaryName, bool _localizeSecondary)
        {
            return ChatHook.Hook(_cInfo, _type, _msg, _mainName, _localizeMain, _secondaryName, _localizeSecondary);
        }

        //public override void GameAwake()
        //{
        //}

        //public override void GameShutdown()
        //{
        //}

        //public override void GameUpdate()
        //{
        //}

        //public override void SavePlayerData(ClientInfo _cInfo, PlayerDataFile _playerDataFile)
        //{
        //}

        //public override void PlayerLogin(ClientInfo _cInfo, string _compatibilityVersion)
        //{
        //}

        //public override void PlayerSpawning(ClientInfo _cInfo, int _chunkViewDim, PlayerProfile _playerProfile)
        //{
        //}

        //public override void PlayerDisconnected(ClientInfo _cInfo, bool _bShutdown)
        //{
        //}
    }
}
