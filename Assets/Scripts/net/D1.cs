//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: d1.proto
namespace d1
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CS_Login")]
  public partial class CS_Login : global::ProtoBuf.IExtensible
  {
    public CS_Login() {}
    
    private string _acc = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"acc", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string acc
    {
      get { return _acc; }
      set { _acc = value; }
    }
    private string _pwd = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"pwd", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string pwd
    {
      get { return _pwd; }
      set { _pwd = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"PacketTyper")]
    public enum PacketTyper
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"PB_PackType", Value=20001)]
      PB_PackType = 20001
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"PacketRetTyper")]
    public enum PacketRetTyper
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"PB_PackRetType", Value=20002)]
      PB_PackRetType = 20002
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SC_LoginResult")]
  public partial class SC_LoginResult : global::ProtoBuf.IExtensible
  {
    public SC_LoginResult() {}
    
    private string _result = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string result
    {
      get { return _result; }
      set { _result = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}