using System;

namespace BuilderExamples
{ 
    internal class Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            return $"From: {From}; \nTo: {To}; \nSubject: {Subject}; \nBody: {Body}";
        }
    }

    internal interface IEmailBuilder
    {
        public Email Build();
    }

    internal interface IBodyBuilder
    {
        public IEmailBuilder ThatSays(string body);
    }

    internal interface ISubjectBuilder
    {
        public IBodyBuilder WithSubject(string subject);
    }

    internal interface IToBuilder
    {
        public ISubjectBuilder To(string to);
    }

    internal interface IFromBuilder
    {
        public IToBuilder From(string from);
    }

    /// <summary>
    /// Creates a public static API to trigger the chain, then relies
    /// on the private implementation class to step through the chain,
    /// as defined by the interface contracts, while building the Email
    /// object to return.
    /// </summary>
    internal class EmailBuilder
    {
        public static IFromBuilder Create()
        {
            return new Impl();
        }

        private class Impl :
            IFromBuilder,
            IToBuilder,
            ISubjectBuilder,
            IBodyBuilder,
            IEmailBuilder
        {
            private Email email = new Email();

            public IToBuilder From(string from)
            {
                email.From = from;
                return this;
            }

            public ISubjectBuilder To(string to)
            {
                email.To = to;
                return this;
            }

            public IBodyBuilder WithSubject(string subject)
            {
                email.Subject = subject;
                return this;
            }

            public IEmailBuilder ThatSays(string body)
            {
                email.Body = body;
                return this;
            }

            public Email Build()
            {
                return email;
            }
        }
    }

    class StepwiseBuilder
    {
        static void Main(string[] args)
        {
            var email = EmailBuilder.Create()
                .From("kyle.loyd@ally.com")
                .To("ben.herron@ally.com")
                .WithSubject("Coconuts")
                .ThatSays("Food or weapon?  Nobody knows.")
                .Build();

            Console.WriteLine(email);
            Console.ReadLine();
        }
    }
}
