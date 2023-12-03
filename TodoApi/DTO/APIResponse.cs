using Microsoft.CodeAnalysis.Elfie.Extensions;

public class APIResponse<T>
{
    public APIResponse()
    {
        DateTime currentTime = DateTime.Now;
        long timeString = currentTime.ToLong();
        Timestamp = timeString;
        Code = 200;
        Msg = "success";
    }

    public APIResponse(T obj)
    {
        DateTime currentTime = DateTime.Now;
        long timeString = currentTime.ToLong();
        Timestamp = timeString; Data = obj;
        Code = 200;
        Msg = "success";
    }
    public APIResponse(T obj, string msg)
    {
        DateTime currentTime = DateTime.Now;
        long timeString = currentTime.ToLong();
        Timestamp = timeString; Data = obj;
        Code = 200;
        Msg = msg;
    }
    public APIResponse(int code, string msg)
    {
        DateTime currentTime = DateTime.Now;
        long timeString = currentTime.ToLong();
        Timestamp = timeString;
        Code = code;
        Msg = msg;
    }
    public APIResponse(int code, string msg, T obj)
    {
        DateTime currentTime = DateTime.Now;
        long timeString = currentTime.ToLong();
        Timestamp = timeString;
        Code = code;
        Msg = msg;
        Data = obj;
    }
    public int Code { get; set; }
    public string? Msg { get; set; }
    public T? Data { get; set; }
    public string? Trace { get; set; }
    public long? Timestamp { get; set; } = DateTime.Now.ToLong();


}