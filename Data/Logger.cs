using System;
using System.Xml;
using System.Collections.Generic;
using System.Threading;
using System.Numerics;

namespace Data
{
    internal class Logger
    {
        private static Logger _instance;
        private static readonly object _lock = new object();

        private Queue<Action> queue = new Queue<Action>();
        private AutoResetEvent hasNewItems = new AutoResetEvent(false);
        private XmlWriter writer = XmlWriter.Create("log.xml", new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            Indent = true
        });
        private Thread loggingThread;

        private Logger()
        {
            writer.WriteStartElement("Info");
            loggingThread = new Thread(new ThreadStart(ProcessQueue));
            loggingThread.IsBackground = true;
            loggingThread.Start();
        }

        ~Logger()
        {
            loggingThread.Join();
            writer.WriteEndElement();
            writer.Close();
        }

        public static Logger GetInstance()
        {
            // Avoid synchronization when the object has already been constructed
            if (_instance == null)
            {
                lock(_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                    }
                }
            }
            return _instance;
        }

        private void ProcessQueue()
        {
            while (true)
            {
                // Wait until there are new items in the queue
                hasNewItems.WaitOne(100, true);

                Queue<Action> queueCopy;
                lock (queue)
                {
                    queueCopy = new Queue<Action>(queue);
                    queue.Clear();
                }

                foreach (Action log in queueCopy)
                {
                    log();
                }
            }
        }

        public void LogBallPosition(Vector2 pos)
        {
            lock (queue)
            {
                queue.Enqueue(() => LogBallPositionAsXML(pos));
            }
            
            // Signal that there are new items in the queue so that the thread
            // running ProcessQueue() can restore execution
            hasNewItems.Set();
        }

        private void LogBallPositionAsXML(Vector2 pos)
        {
            writer.WriteStartElement("Ball");
            writer.WriteElementString("X", XmlConvert.ToString(pos.X));
            writer.WriteElementString("Y", XmlConvert.ToString(pos.Y));
            writer.WriteEndElement();
        }
    }
}
