//https://www.jazz2online.com/wiki/J2A_File_Format by Neobeo
using System;
using System.IO;
using JJ2AnimLib.JJ2AnimSections;

namespace JJ2AnimLib
{
    public class JJ2AnimFile
    {
        public ALIB_Header Header { get; set; }
        public AnimSet[] Sets = new AnimSet[0]; 
    

        private bool _success = false;
        public bool Success { get { return _success; } }


        public JJ2AnimFile()
        {
        }


        public JJ2AnimFile(string file)
        {
            Load(file);
            
        }

    public bool Load(string file)
        {
            bool res = false;
            byte[] buff = File.ReadAllBytes(file);
            Header = new ALIB_Header();
            if (Header.Read(buff))
            {
                if (ReadSets(buff, Header.GetSize))
                {

                }
            }
            _success = res;
            return res;
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

            }
            return true;
        }
    }
}
