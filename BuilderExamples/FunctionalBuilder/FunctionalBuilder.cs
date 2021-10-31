using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalBuilder
{
    public class Email
    {
        public string To, From, Subject, Body;
        public List<Attachment> Attachments = new();

        public override string ToString()
        {
            return 
                $"From: {From}\n" +
                $"To: {To};\n" +
                $"Subject: {Subject};\n" +
                $"Body: {Body};\n" +
                $"Attachments: {Attachments.Select(a => a.Filename).Aggregate((i, j) => i + ", " + j)}";
        }
    }

    public enum AttachmentType
    {
        Text,
        Image,
        Zip
    }

    public class Attachment
    {
        public string Filename;
        public AttachmentType Type;
    }

    internal abstract class FunctionalBuilder<TObj, TChild>
        where TObj : new() // <-- No idea what this line is for
        where TChild : FunctionalBuilder<TObj, TChild>
    {
        private readonly List<Action<TObj>> actions = new();

        protected TChild AddAction(Action<TObj> action) // Don't know how to handle protection level
        {
            actions.Add(action);
            return (TChild) this;
        }

        public TObj Build()
        {
            var obj = new TObj();
            actions.ForEach(a => a(obj));
            return obj;
        }
    }

    internal class EmailBuilder : FunctionalBuilder<Email, EmailBuilder>
    {
        public EmailBuilder AddAction(Action<Email> action) 
            => base.AddAction(action);

        public EmailBuilder To(string to) 
            => AddAction(e => e.To = to);

        public EmailBuilder From(string from)
            => AddAction(e => e.From = from);

        public EmailBuilder WithSubject(string subject) 
            => AddAction(e => e.Subject = subject);

        public EmailBuilder WithBody(string body)
            => AddAction(e => e.Body = body);
    }

    internal static class EmailBuilderExtensions
    {
        public static EmailBuilder AddAttachment
            (this EmailBuilder builder, Attachment attachment)
        {
            builder.AddAction(e => e.Attachments.Add(attachment));
            return builder;
        }
    }

    class FunctionalBuilder
    {
        static void Main(string[] args)
        {
            var email = new EmailBuilder()
                .To("ben.herron@ally.com")
                .From("kyle.loyd@ally.com")
                .WithSubject("Coconuts")
                .WithBody("Weapon or food?  I'm leaning towards weapon.")
                .AddAttachment(new Attachment { 
                    Filename = "chungus.jpg",
                    Type = AttachmentType.Image
                })
                .AddAttachment(new Attachment
                {
                    Filename = "the-tale-of-chungus.txt",
                    Type = AttachmentType.Text
                })
                .Build();
            Console.WriteLine(email);
        }
    }
}
