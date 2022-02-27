
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectionController : MonoBehaviour
{
    public static ConnectionController instance;

    public bool debug;

    public Dictionary<Connection, ConnectionRenderer> connections = new Dictionary<Connection, ConnectionRenderer>();

    public List<ConnectionPath> connectionPaths = new List<ConnectionPath>();
    
    private void Awake()
    {
        Configure();
    }

    private void Update()
    {
        
        
    }

    public void CreateConnectionPathFromString(string _connectPathString)
    {
        Log("Attempting to make a path from string "+_connectPathString);
        if (_connectPathString == String.Empty) return;
        
        ConnectionPath _connectionPath = new ConnectionPath();
        for (int i = 0; i < _connectPathString.Length; i++)
        {
            if (i + 1 < _connectPathString.Length)
            {
                string _topKeyholeValue = _connectPathString.Substring(i, 1);
                Keyhole _topKeyhole = KeyholeController.instance.GetKeyholeByValue(_topKeyholeValue);
                string _bottomKeyholeValue = _connectPathString.Substring(i+1, 1);
                Keyhole _bottomKeyhole = KeyholeController.instance.GetKeyholeByValue(_bottomKeyholeValue);
                if (ConnectNodes(_topKeyhole, _bottomKeyhole))
                {
                    _connectionPath.connections.Add(new Connection(_topKeyhole,_bottomKeyhole));
                } 
            }
        }

        for(int i=0;i<_connectionPath.connections.Count;i++)
        {
            _connectionPath.pathString += _connectionPath.connections[i].top.value;
            if(i+1 == _connectionPath.connections.Count) 
                _connectionPath.pathString += _connectionPath.connections[i].bottom.value;
        }
        connectionPaths.Add(_connectionPath);
    }
    
    public void DisconnectKeyhole(string _keyholeValue)
    {
        _keyholeValue = _keyholeValue.ToUpper();
        Keyhole _disconnectingKeyhole = KeyholeController.instance.GetKeyholeByValue(_keyholeValue);
        Log("Attempting to disconnect keyhole "+_keyholeValue);
        foreach (KeyValuePair<Connection,ConnectionRenderer> _connection in connections.ToList())
        {
            if (_connection.Key.top.value == _keyholeValue || _connection.Key.bottom.value == _keyholeValue)
            {
                ConnectionRendererPool.instance.Release(_connection.Value);
                connections.Remove(_connection.Key);
            }
        }
        
        foreach (ConnectionPath _connectionPath in connectionPaths.ToList())
        {
            if (_connectionPath.pathString.Contains(_keyholeValue))
            {
                string[] _newPathStrings = _connectionPath.pathString.Split(_keyholeValue);
                connectionPaths.Remove(_connectionPath);
                foreach (string _pathString in _newPathStrings)
                {
                    string _replacedPathString = _pathString.Replace(_keyholeValue, String.Empty);
                    CreateConnectionPathFromString(_replacedPathString);
                }
            }
        }
        
        _disconnectingKeyhole.connectedNodes.Clear();
        foreach (Keyhole _keyhole in KeyholeController.instance.keyholes)
        {
            if (_keyhole.connectedNodes.Contains(_disconnectingKeyhole))
            {
                _keyhole.connectedNodes.Remove(_disconnectingKeyhole);
            }
        }
    }

    public void AddConnection(Connection _connection)
    {
        Log("Adding connection between "+_connection.top.value+" and "+_connection.bottom.value);
        connections.Add(_connection, ConnectionRendererPool.instance.Spawn(_connection));
    }

    public void RemoveConnection(KeyValuePair<Connection, ConnectionRenderer> _connection)
    {
        Log("Removing connection between "+_connection.Key.top.value+" and "+_connection.Key.bottom.value);
        _connection.Key.top.connectedNodes.Remove(_connection.Key.bottom);
        _connection.Key.bottom.connectedNodes.Remove(_connection.Key.top);
        ConnectionRendererPool.instance.Release(_connection.Value);
        connections.Remove(_connection.Key);
    }
    
    public bool ConnectNodes(Keyhole _keyhole1, Keyhole _keyhole2)
    {
        Log("Attempting to connect nodes "+_keyhole1.value+" and "+_keyhole2.value);
        bool _doesConnectionAlreadyExist = false;
        if(!_keyhole1.connectedNodes.Contains(_keyhole2)) _keyhole1.connectedNodes.Add(_keyhole2);
        if(!_keyhole2.connectedNodes.Contains(_keyhole1)) _keyhole2.connectedNodes.Add(_keyhole1);

        foreach (KeyValuePair<Connection,ConnectionRenderer> _connection in connections)
        {
            if((_connection.Key.top==_keyhole1 && _connection.Key.bottom==_keyhole2)
                ||(_connection.Key.top==_keyhole2 && _connection.Key.bottom==_keyhole1))
            {
                _doesConnectionAlreadyExist = true;
            }
        }
        
        if(_doesConnectionAlreadyExist)
        {
            return false;
        }
        else
        {
            AddConnection(new Connection(_keyhole1, _keyhole2));
            return true;
        }
    }

    private void Configure()
    {
        if (!instance)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    private void Log(string _msg)
    {
        if(debug) Debug.Log("[ConnnectionController]: "+_msg);
    }
}
