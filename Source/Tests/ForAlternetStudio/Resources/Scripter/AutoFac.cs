using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Autofac;

public interface IMobileServive
{
    void Execute();
}
public class SMSService : IMobileServive
{
    public void Execute()
    {
        ScriptGlobalClass.MainForm.Text += " - SMS service";
    }
}

public interface IMailService
{
    void Execute();
}

public class EmailService : IMailService
{
    public void Execute()
    {
        ScriptGlobalClass.MainForm.Text += "- Email service";
    }
}

public class NotificationSender
{
    public IMobileServive ObjMobileSerivce = null;
    public IMailService ObjMailService = null;

    //injection through constructor  
    public NotificationSender(IMobileServive tmpService)
    {
        ObjMobileSerivce = tmpService;
    }
    //Injection through property  
    public IMailService SetMailService
    {
        set { ObjMailService = value; }
    }
    public void SendNotification()
    {
        ObjMobileSerivce.Execute();
        ObjMailService.Execute();
    }
}

namespace Client
{
    class Program
    {
        static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SMSService>().As<IMobileServive>();
            builder.RegisterType<EmailService>().As<IMailService>();
            var container = builder.Build();

            container.Resolve<IMobileServive>().Execute();
            container.Resolve<IMailService>().Execute();
        }
    }
}