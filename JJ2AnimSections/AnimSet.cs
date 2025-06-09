using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using static System.BitConverter;

namespace JJ2AnimLib.JJ2AnimSections
{
    public class AnimSet
    {
        public ANIM_Header Header { get; set; }
        public AnimInfo[] Animations { get; set; }
        public FrameInfo[] Frames { get; set; }
        public ImageData Images { get; set; }
        public SampleData[] Samples { get; set; }

        private byte[] Data1 = { };
        private byte[] Data2 = { };
        private byte[] Data3 = { };
        private byte[] Data4 = { };

        public byte[] GetSAnimationInfoBuffer => Data1;
        public byte[] GetFrameInfoBuffer => Data2;
        public byte[] GetImageDataBuffer => Data3;
        public byte[] GetSampleDataBuffer => Data4;

        private sbyte _loadPercentage = -1;
        public sbyte LoadPercentage => _loadPercentage;


        public int GetSize
        {
            get { return Header != null? Header.GetSize + Header.CData1 + Header.CData2 + Header.CData3 + Header.CData4 : 0; }
        }

        public void FreeDataBuffers()
        {            
            Data1 = new byte[] { };
            Data2 = new byte[] { };
            Data3 = new byte[] { };
            Data4 = new byte[] { };
        }

        public bool Read(byte[] mem, int offset, bool prepareImages = true)
        {
            Header = new ANIM_Header();
            if (Header.Read(mem,offset))
            {
                _loadPercentage = 0;

                offset += Header.GetSize;
                byte[] compressedData1 = new byte[Header.CData1];
                Array.Copy(mem, offset, compressedData1, 0, Header.CData1);
                Data1 = new byte[Header.UData1];
                long uDataLength = Header.UData1;
                var uncompRes = GeneralFunctions.UncompressByteArray(Data1, ref uDataLength, compressedData1, Header.CData1);
                if (uncompRes != 0)
                    return false;
                offset += Header.CData1;
                _loadPercentage = 5;

                byte[] compressedData2 = new byte[Header.CData2];
                Array.Copy(mem, offset, compressedData2, 0, Header.CData2);
                Data2 = new byte[Header.UData2];
                uDataLength = Header.UData2;
                if (GeneralFunctions.UncompressByteArray(Data2, ref uDataLength, compressedData2, Header.CData2) != 0)
                    return false;
                offset += Header.CData2;
                _loadPercentage = 10;

                byte[] compressedData3 = new byte[Header.CData3];
                Array.Copy(mem, offset, compressedData3, 0, Header.CData3);
                Data3 = new byte[Header.UData3];
                uDataLength = Header.UData3;
                if (GeneralFunctions.UncompressByteArray(Data3, ref uDataLength, compressedData3, Header.CData3) != 0)
                    return false;
                offset += Header.CData3;
                _loadPercentage = 15;

                byte[] compressedData4 = new byte[Header.CData4];
                Array.Copy(mem, offset, compressedData4, 0, Header.CData4);
                Data4 = new byte[Header.UData4];
                uDataLength = Header.UData4;
                if (GeneralFunctions.UncompressByteArray(Data4, ref uDataLength, compressedData4, Header.CData4) != 0)
                    return false;
                offset += Header.CData4;
                _loadPercentage = 20;

                /*
                Data1 = GeneralFunctions.Decompress(compressedData1);
                Data1 = GeneralFunctions.Unzip(mem, offset, Header.CData1);
                offset+= Header.CData1;
                Data1 = GeneralFunctions.Unzip(mem, offset, Header.CData2);
                offset += Header.CData2;
                Data1 = GeneralFunctions.Unzip(mem, offset, Header.CData3);
                offset += Header.CData3;
                Data1 = GeneralFunctions.Unzip(mem, offset, Header.CData4);
                offset += Header.CData4;
                */

                if (ReadAnimations(this.Data1) == false)
                    return false;
                _loadPercentage = 30;
                if (ReadFrames(this.Data2) == false)
                    return false;
                _loadPercentage = 50;
                if (prepareImages && ReadImages(this.Frames, this.Data3) == false)
                    return false;
                _loadPercentage = 80;
                if (ReadSamples(this.Samples, this.Data4) == false)
                    return false;
                _loadPercentage = 100;

                // System.IO.File.WriteAllBytes("D:/IMG3.DAT", this.Data3);
                // System.IO.File.WriteAllBytes("D:/MASK4.DAT", this.Data4);
                return true;
            }

            ReadFail:
            return false;
        }

        private bool ReadAnimations(byte[] buff, int offset = 0) //Read Data1
        {
            Animations = new AnimInfo[Header.AnimationCount];
            int startFrame = 0;
            for(int i = 0; i < Animations.Length; i++)
            {
                Animations[i] = new AnimInfo((short)(buff[offset] | (buff[offset +1]<< 8)), (short)(buff[offset + 2] | (buff[offset + 3] << 8)), buff[offset+4] | (buff[offset + 5] << 8) | (buff[offset + 6] << 16) | (buff[offset + 7] << 24), startFrame);
                startFrame += Animations[i].FrameCount;
                 offset += AnimInfo.SIZE;
            }
            return true;
        }

        private bool ReadFrames(byte[] buff, int offset = 0) //Read Data2
        {
            Frames = new FrameInfo[Header.FrameCount];
            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i] = new FrameInfo(ToInt16(buff, offset), ToInt16(buff, offset + 2), ToInt16(buff, offset + 4), ToInt16(buff, offset+ 6), ToInt16(buff, offset+8), ToInt16(buff, offset+10), ToInt16(buff, offset + 12), ToInt16(buff, offset+14),ToInt32(buff, offset +16), ToInt32(buff,offset + 20));
                offset += FrameInfo.SIZE;
            }
            return true;
        }

        private bool ReadImages(FrameInfo[] destFrames, byte[] imgBuff, int offset = 0)
        {
            for(int f = 0; f < destFrames.Length; f++)
            {
                destFrames[f].Img8Bit = new byte[destFrames[f].Width, destFrames[f].Height];
                destFrames[f].TMask = new byte[destFrames[f].Width, destFrames[f].Height];
                int imgAddress = offset + destFrames[f].ImageAddress;

                short w = BitConverter.ToInt16(imgBuff, imgAddress);
                short h = BitConverter.ToInt16(imgBuff, imgAddress + 2);
                bool isTranslucent = (w & 0x8000) != 0;
                w = (short)(w & 0x7FFF);

                
                int x = 0;
                int y = 0;
                int i = imgAddress + 4;
                byte codebyte;
                int count;
                while (y < h)
                {
                    codebyte = imgBuff[i++];
                    if(codebyte != 0x80)
                    {
                        count = codebyte & 0x7F;
                        if(codebyte > 0x80) //read "count" times
                        {
                            for(int n = 0; n < count; n++)
                            {
                                if (x >= w)
                                {
                                    x = 0;
                                    y++;
                                    //if(y >= h)

                                }
                                destFrames[f].Img8Bit[x, y] = imgBuff[i++];
                                destFrames[f].TMask[x, y] = 1;
                                x++;
             
                            }
                        }
                        else //skip "count" times
                        {
                            y += count / w;
                            x += count % w;
                            
                        }
                    }
                    else //next row
                    {
                        x = 0;
                        y++;
                        //i++;
                    }

                }
                
                for(i = 0; i < destFrames[f].Width; i++)
                    for(int j = 0; j < destFrames[f].Height; j++)
                        destFrames[f].TMask[i, j] = destFrames[f].TMask[i, j] == 0 ? (byte)1 : (byte)0; //inverse

                _loadPercentage = (sbyte)(50 + (f / (float)destFrames.Length * (80 - 50)));
            }

            return true;
        }

        private bool ReadSamples(SampleData[] destSamples, byte[] sampBuff, int offset = 0)
        {
            Samples = new SampleData[Header.SampleCount];
            int sampleID = Header.PriorSampleCount;
            int i = offset;
            for (int s = 0; s <  Header.SampleCount; s++)
            {
                Samples[s] = new SampleData(ToInt32(sampBuff, i), sampleID++); //SampleSize = first int in sample header - 4
                Array.Copy(sampBuff, i + sizeof(int), Samples[s].Buffer, 0, Samples[s].SampleSize);
                i += Samples[s].TotalSize;
                _loadPercentage = (sbyte)(80 + (s / (float)Header.SampleCount * (100 - 80)));
            }

            return true;
        }
       
    }
}
