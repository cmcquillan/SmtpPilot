using SmtpPilot.Server;
using SmtpPilot.Tests.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Tests
{
    internal static class TestHelper
    {
        internal static SmtpPilotConfiguration GetConfig(string text)
        {
            var ms = new MemoryStream(Encoding.ASCII.GetBytes(text));
            var listener = new TextClientListener(ms);
            var config = new SmtpPilotConfiguration(new[] { listener }, "Test");

            return config;
        }

        public static string BasicMessage =
@"EHLO bar.com
MAIL FROM:<Smith@bar.com>
RCPT TO:<Jones@foo.com>
RCPT TO:<Green@foo.com>
RCPT TO:<Brown@foo.com>
DATA
Blah blah blah...
...etc. etc. etc.
.
QUIT";

        public static string LongMessage =
@"EHLO bar.com
MAIL FROM:<Smith@bar.com>
RCPT TO:<Jones@foo.com>
RCPT TO:<Green@foo.com>
RCPT TO:<Brown@foo.com>
DATA
But we've met before. That was a long time ago, I was a kid at St. Swithin's, It used to be funded by the Wayne Foundation. It's an orphanage. My mum died when I was small, it was a car accident. I don't remember it. My dad got shot a couple of years later for a gambling debt. Oh I remember that one just fine. Not a lot of people know what it feels like to be angry in your bones. I mean they understand. The fosters parents. Everybody understands, for a while. Then they want the angry little kid to do something he knows he can't do, move on. After a while they stop understanding. They send the angry kid to a boy's home, I figured it out too late. Yeah I learned to hide the anger, and practice smiling in the mirror. It's like putting on a mask. So you showed up this one day, in a cool car, pretty girl on your arm. We were so excited! Bruce Wayne, a billionaire orphan? We used to make up stories about you man, legends and you know with the other kids, that's all it was, just stories, but right when I saw you, I knew who you really were. I'd seen that look on your face before. It's the same one I taught myself. I don't why you took the fault for Dent's murder but I'm still a believer in the Batman. Even if you're not...
But we've met before. That was a long time ago, I was a kid at St. Swithin's, It used to be funded by the Wayne Foundation. It's an orphanage. My mum died when I was small, it was a car accident. I don't remember it. My dad got shot a couple of years later for a gambling debt. Oh I remember that one just fine. Not a lot of people know what it feels like to be angry in your bones. I mean they understand. The fosters parents. Everybody understands, for a while. Then they want the angry little kid to do something he knows he can't do, move on. After a while they stop understanding. They send the angry kid to a boy's home, I figured it out too late. Yeah I learned to hide the anger, and practice smiling in the mirror. It's like putting on a mask. So you showed up this one day, in a cool car, pretty girl on your arm. We were so excited! Bruce Wayne, a billionaire orphan? We used to make up stories about you man, legends and you know with the other kids, that's all it was, just stories, but right when I saw you, I knew who you really were. I'd seen that look on your face before. It's the same one I taught myself. I don't why you took the fault for Dent's murder but I'm still a believer in the Batman. Even if you're not...
But we've met before. That was a long time ago, I was a kid at St. Swithin's, It used to be funded by the Wayne Foundation. It's an orphanage. My mum died when I was small, it was a car accident. I don't remember it. My dad got shot a couple of years later for a gambling debt. Oh I remember that one just fine. Not a lot of people know what it feels like to be angry in your bones. I mean they understand. The fosters parents. Everybody understands, for a while. Then they want the angry little kid to do something he knows he can't do, move on. After a while they stop understanding. They send the angry kid to a boy's home, I figured it out too late. Yeah I learned to hide the anger, and practice smiling in the mirror. It's like putting on a mask. So you showed up this one day, in a cool car, pretty girl on your arm. We were so excited! Bruce Wayne, a billionaire orphan? We used to make up stories about you man, legends and you know with the other kids, that's all it was, just stories, but right when I saw you, I knew who you really were. I'd seen that look on your face before. It's the same one I taught myself. I don't why you took the fault for Dent's murder but I'm still a believer in the Batman. Even if you're not...
But we've met before. That was a long time ago, I was a kid at St. Swithin's, It used to be funded by the Wayne Foundation. It's an orphanage. My mum died when I was small, it was a car accident. I don't remember it. My dad got shot a couple of years later for a gambling debt. Oh I remember that one just fine. Not a lot of people know what it feels like to be angry in your bones. I mean they understand. The fosters parents. Everybody understands, for a while. Then they want the angry little kid to do something he knows he can't do, move on. After a while they stop understanding. They send the angry kid to a boy's home, I figured it out too late. Yeah I learned to hide the anger, and practice smiling in the mirror. It's like putting on a mask. So you showed up this one day, in a cool car, pretty girl on your arm. We were so excited! Bruce Wayne, a billionaire orphan? We used to make up stories about you man, legends and you know with the other kids, that's all it was, just stories, but right when I saw you, I knew who you really were. I'd seen that look on your face before. It's the same one I taught myself. I don't why you took the fault for Dent's murder but I'm still a believer in the Batman. Even if you're not...
But we've met before. That was a long time ago, I was a kid at St. Swithin's, It used to be funded by the Wayne Foundation. It's an orphanage. My mum died when I was small, it was a car accident. I don't remember it. My dad got shot a couple of years later for a gambling debt. Oh I remember that one just fine. Not a lot of people know what it feels like to be angry in your bones. I mean they understand. The fosters parents. Everybody understands, for a while. Then they want the angry little kid to do something he knows he can't do, move on. After a while they stop understanding. They send the angry kid to a boy's home, I figured it out too late. Yeah I learned to hide the anger, and practice smiling in the mirror. It's like putting on a mask. So you showed up this one day, in a cool car, pretty girl on your arm. We were so excited! Bruce Wayne, a billionaire orphan? We used to make up stories about you man, legends and you know with the other kids, that's all it was, just stories, but right when I saw you, I knew who you really were. I'd seen that look on your face before. It's the same one I taught myself. I don't why you took the fault for Dent's murder but I'm still a believer in the Batman. Even if you're not...
.
QUIT";
    }
}
