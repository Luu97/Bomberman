using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GlobalStateManager : NetworkBehaviour {

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private List<Player> alivePlayers = new List<Player>();

    public static void RegisterPlayer (string _netID, Player _player) {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer (string _playerID) {
        players.Remove(_playerID);
    }

    public static Player GetPlayer (string _playerID) {
        return players[_playerID];
    }

    public static List<Player> GetAllPlayers () {
        List<Player> allPlayers = new List<Player>();
        foreach (string playerKey in players.Keys) {
            allPlayers.Add(players[playerKey]);
        }
        return allPlayers;
    }

    public static List<Player> GetAlivePlayers () {
        List<Player> alivePlayers = new List<Player>();
        foreach (string playerKey in players.Keys) {
            if (!players[playerKey].Dead) {
                alivePlayers.Add(players[playerKey]);
            }
            
        }
        return alivePlayers;
    }
}
