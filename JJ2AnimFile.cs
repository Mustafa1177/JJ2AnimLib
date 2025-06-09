//https://www.jazz2online.com/wiki/J2A_File_Format article by Neobeo
using System;
using System.IO;
using System.Threading;
using JJ2AnimLib.JJ2AnimSections;

namespace JJ2AnimLib
{
    public class JJ2AnimFile
    {
        public ALIB_Header Header { get; set; }

        public AnimSet[] Sets = new AnimSet[0]; 
    
        private bool _success = false;

        private bool _busy = false;

        private Thread _loadThread;

        private sbyte _loadPercentage = -1;

        public bool Success { get { return _success; } }

        public bool Busy { get { return _busy; } }

        public sbyte LoadPercentage { get { return _loadPercentage; } }
      


        public JJ2AnimFile()
        {
            _loadThread = new Thread(ThreadedLoad);
        }

        public JJ2AnimFile(string file)
        {
            _loadThread = new Thread(ThreadedLoad);
            Load(file);        
        }

        public bool Load(byte[] buff)
        {
            bool res = false;
            _loadPercentage = 0;
            Header = new ALIB_Header();
            if (Header.Read(buff))
            {
                if (ReadSets(buff, Header.GetSize))
                {
                    res = true;
                }
            }
            _success = res;
            return res;
        }

        public bool Load(string file)
        {
            bool res = false;
            byte[] buff = File.ReadAllBytes(file);
            res = Load(buff);
            return res;
        }

        // untested
        public bool LoadAsync(byte[] buff)
        {
            if(!_busy)
            {
                _busy = true;
                if(_loadThread.ThreadState != ThreadState.Running)
                {
                    _loadThread = new Thread(ThreadedLoad);
                    _threadBuff = buff;
                    _loadThread.Start();
                    return true;
                }    
                else
                    _busy = false;
            }
            return false;
        }

        byte[] _threadBuff;
        private void ThreadedLoad()
        {
            byte[] buff = _threadBuff;
            if(buff != null)
            {
                _threadBuff = null;
                Load(buff);
            }
            _busy = false;
        }



    private bool ReadSets(byte[] mem, int offset)
        {
            Sets = new AnimSet[Header.SetCount];
            for(int i = 0; i < Sets.Length; i++)
            {
                Sets[i] = new AnimSet();
                if (!Sets[i].Read(mem, Header.SetAddress[i]))
                    return false;
                // address/offset can be calculated manually as: offset += Sets[i].GetSize
                // but we will use addresses given in the file instead (Header.SetAddress[i]).
                _loadPercentage = (sbyte)(i / (float)Sets.Length * 100);
            }
            return true;
        }
    }
}
