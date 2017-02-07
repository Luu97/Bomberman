using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<int, Player> players = new Dictionary<int, Player>();

    public static void RegisterPlayer(int _id, Player _player) {
        string _playerID = PLAYER_ID_PREFIX + _id;
        if (!players.ContainsKey(_id))
            players.Add(_id, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(int _id) {
        players.Remove(_id);
    }

    public static Player GetPlayer(int _id) {
        if (players[_id] == null)
            return null;
        return players[_id];
    }

    public static List<Player> GetAllPlayers() {
        List<Player> allPlayers = new List<Player>();
        foreach (int playerKey in players.Keys) {
            allPlayers.Add(players[playerKey]);
        }
        return allPlayers;
    }

    public static List<Player> GetAlivePlayers() {
        List<Player> alivePlayers = new List<Player>();
        foreach (int playerKey in players.Keys) {
            if (!players[playerKey].Dead) {
                alivePlayers.Add(players[playerKey]);
            }

        }
        return alivePlayers;
    }
}
