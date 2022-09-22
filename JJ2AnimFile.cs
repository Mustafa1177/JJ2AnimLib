//https://www.jazz2online.com/wiki/J2A_File_Format by Neobeo
using System;
using System.IO;
using JJ2AnimLib.JJ2AnimSections;

namespace JJ2AnimLib
{
    public class JJ2AnimFile
    {
        public ALIB_Header Header;
        //public ANIM_Header[] SetsHeaders = new ANIM_Header[0];
        public AnimSet[] Sets = new AnimSet[0]; 
    
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
            return res;
        }
    private bool ReadSets(byte[] mem, int offset)
        {
            Sets = new AnimSet[Header.SetCount];
            for(int i = 0; i< Sets.Length; i++)
            {
                Sets[i] = new AnimSet();
               if (!Sets[i].Read(mem,offset))
                    return false;

            }
            return true;
        }
    }
}
