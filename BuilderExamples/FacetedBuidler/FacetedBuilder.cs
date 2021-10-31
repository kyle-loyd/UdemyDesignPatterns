using System;

namespace FacetedBuidler
{
    internal class Email
    {
        // Contacts
        internal string To, From;

        // Content
        internal string Subject, Body;

        public override string ToString()
        {
            return $"From: {From}; \nTo: {To}; \nSubject: {Subject}; \nBody: {Body}";
        }
    }

    internal class EmailBuilder
    {
        protected Email email = new();

        internal EmailContactsBuilder Sent => new(email); // Why arrows?
        internal EmailContentBuilder With => new(email);

        public static implicit operator Email(EmailBuilder b)
        {
            return b.email;
        }
    }

    internal class EmailContactsBuilder : EmailBuilder
    {
        internal EmailContactsBuilder(Email email) =>
            this.email = email;

        internal EmailContactsBuilder To(string to)
        {
            email.To = to;
            return this;
        }

        internal EmailContactsBuilder From(string from)
        {
            email.From = from;
            return this;
        }
    }

    internal class EmailContentBuilder : EmailBuilder
    {
        internal EmailContentBuilder(Email email)
            => this.email = email;

        internal EmailContentBuilder Subject(string subject)
        {
            email.Subject = subject;
            return this;
        }

        internal EmailContentBuilder Body(string body)
        {
            email.Body = body;
            return this;
        }
    }

    class FacetedBuilder
    {
        static void Main(string[] args)
        {
            var builder = new EmailBuilder();
            Email email = builder
                .Sent
                    .To("ben.herron@ally.com")
                    .From("kyle.loyd@ally.com")
                .With
                    .Subject("Coconuts")
                    .Body("Definitely a weapon.  To arms, brothers!");
            Console.WriteLine(email);
        }
    }
}
