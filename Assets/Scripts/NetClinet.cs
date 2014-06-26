using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Threading;

public class NetClinet :MonoBehaviour
{

	TcpClient tcp = null;
	NetworkStream workStream = null;
	public ManualResetEvent connectDone = new ManualResetEvent(false);

	delegate void SetTextCallback(string text);
	delegate void SetControl();
	delegate void GetData(byte[] data);
	
	/// <summary>
	/// 异步接收数据
	/// </summary>
	/// <param name="data"></param>
	private void OnGetData(byte[] data)
	{
		/*
		string sdata;
		if (CurrentReceiveDataMode == DataMode.Text)
		{
			sdata = new string(Encoding.UTF8.GetChars(data));
		}
		else
		{
			sdata = ByteArrayToHexString(data);
		}
		
		rtfReceive.Invoke(new EventHandler(delegate
		                                   {
			rtfReceive.AppendText(sdata);
		}));*/
	}
	

	//public ManualResetEvent connectDone = new ManualResetEvent(false);
	/// <summary>
	/// 异步连接的回调函数
	/// </summary>
	/// <param name="ar"></param>
	private void ConnectCallback(IAsyncResult ar)
	{
		connectDone.Set();
		TcpClient t = (TcpClient)ar.AsyncState;
		try
		{
			if (t.Connected)
			{
				Debug.Log("连接成功");
				t.EndConnect(ar);
				Debug.Log("连接线程完成");
			}
			else
			{
				Debug.Log("连接失败");
				t.EndConnect(ar);
			}
			
		}
		catch (SocketException se)
		{
			Debug.Log("连接发生错误ConnCallBack.......:"+se.Message);
		}
	}
	
	/// <summary>
	/// 异步连接
	/// </summary>
	public void Connect()
	{
		if ((tcp == null) || (!tcp.Connected))
		{
			try
			{
				tcp = new TcpClient();
				tcp.ReceiveTimeout = 10;
				connectDone.Reset();
				tcp.BeginConnect("127.0.0.1", 8888, new AsyncCallback(ConnectCallback), tcp);

				connectDone.WaitOne();
				
				if ((tcp != null) && (tcp.Connected))
				{
					workStream = tcp.GetStream();
					Debug.Log("Connection established");
					asyncRead(tcp);
				}
			}
			catch (Exception se)
			{
				Debug.Log(se.Message+" Conn......."+Environment.NewLine);
			}
		}
	}
	
	/// <summary>
	/// 断开连接
	/// </summary>	
	public void DisConnect()
	{
		if ((tcp != null) && (tcp.Connected))
		{
			workStream.Close();
			tcp.Close();
		}
	}
	
	
	/// <summary>
	/// 异步读TCP数据
	/// </summary>
	/// <param name="sock"></param>
	private void asyncRead(TcpClient sock)
	{
		StateObject state = new StateObject();
		state.client = sock;
		NetworkStream stream = sock.GetStream();
		
		if (stream.CanRead)
		{
			try
			{
				IAsyncResult ar = stream.BeginRead(state.buffer, 0, StateObject.BufferSize,
				                                   new AsyncCallback(TCPReadCallBack), state);
			}
			catch (Exception e)
			{
				Debug.Log("Network IO problem " + e.ToString());
			}
		}
	}
	
	/// <summary>
	/// TCP读数据的回调函数
	/// </summary>
	/// <param name="ar"></param>
	private void TCPReadCallBack(IAsyncResult ar)
	{
		StateObject state = (StateObject)ar.AsyncState;
		//主动断开时
		if ((state.client == null) || (!state.client.Connected))
			return;
		int numberOfBytesRead;
		NetworkStream mas = state.client.GetStream();
		string type = null;
		
		numberOfBytesRead = mas.EndRead(ar);
		state.totalBytesRead += numberOfBytesRead;
		
		Debug.Log("Bytes read ------ "+ numberOfBytesRead.ToString());

		if (numberOfBytesRead > 0)
		{
			byte[] dd = new byte[numberOfBytesRead];
			Array.Copy(state.buffer,0,dd,0,numberOfBytesRead);
			OnGetData(dd);
			mas.BeginRead(state.buffer, 0, StateObject.BufferSize,
			              new AsyncCallback(TCPReadCallBack), state);
		}
		else
		{
			//被动断开时 
			//mas.Close();
			//state.client.Close();
			//Debug.Log("Bytes read ------ "+ numberOfBytesRead.ToString());
			//Debug.Log("不读了");
			//mas = null;
			//state = null;
		}
	}
	
}

internal class StateObject
{
	public TcpClient client = null;
	public int totalBytesRead = 0;
	public const int BufferSize = 1024;
	public string readType = null;
	public byte[] buffer = new byte[BufferSize];
	//public StringBuilder messageBuffer = new StringBuilder();
}


