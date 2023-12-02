using Microsoft.CodeAnalysis.Elfie.Extensions;

class APIResponse<T>
{
    public APIResponse()
    {
    }

    public APIResponse(T obj)
    {
        Timestamp = new DateTime().ToLong();
        Data = obj;
        Code = 200;
        Msg = "success";
    }
    public APIResponse(T obj, string msg)
    {
        Timestamp = new DateTime().ToLong();
        Data = obj;
        Code = 200;
        Msg = msg;
    }
    public APIResponse(int code, string msg)
    {
        Timestamp = new DateTime().ToLong();
        Code = code;
        Msg = msg;
    }
    public int Code { get; set; }
    public string? Msg { get; set; }
    public T? Data { get; set; }
    public string? Trace { get; set; }
    public long? Timestamp { get; set; }


}