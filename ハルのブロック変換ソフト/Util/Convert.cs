using SilverNBTLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SilverNBTLibrary.World;

namespace ハルのブロック変換ソフト.Util
{
    class Convert
    {
        public ProgressBar pgb { set; get; }
        public Form1 form { set; get; }
        public BackgroundWorker bw { set; get; }


        public void ToStructure(string file)
        {
            var loadtag = NBTFile.LoadFromFile(file);
            var schematic = SilverNBTLibrary.Structure.Schematic.LoadFromNBT(loadtag);
            var structure = new SilverNBTLibrary.Structure.StructureTemplate(schematic.Width, schematic.Height, schematic.Length);
            structure.Author = "Haru";

            form.Invoke((MethodInvoker)delegate ()
            {
                pgb.Maximum = schematic.Width * schematic.Height * schematic.Length;
                pgb.Value = 0;
            });
            for (int x = 0; x < schematic.Width; x++)
            {
                for (int y = 0; y < schematic.Height; y++)
                {
                    for (int z = 0; z < schematic.Length; z++)
                    {
                        var id = schematic.GetBlockId(x, y, z);
                        var meta = schematic.GetBlockMetadata(x, y, z);
                        structure.SetBlock(x, y, z, id, meta);
                        bw.ReportProgress(0);
                    }
                }
            }

            foreach (NBTTagCompound te in schematic.TileEntities)
            {
                structure.AddTileEntityTag(te.GetTagInt("x").Value, te.GetTagInt("y").Value, te.GetTagInt("z").Value, te);
            }
            foreach (NBTTagCompound entity in schematic.Entities)
            {
                structure.Entities.Add(entity);
            }
            var nbt = structure.SaveToNBT();
            NBTFile.SaveToFile(Path.GetDirectoryName(file) + @"\" + Path.GetFileNameWithoutExtension(file) + ".nbt", nbt);

        }

        public void ToSchematic(string file)
        {
            var loadtag = NBTFile.LoadFromFile(file);
            var structure = SilverNBTLibrary.Structure.StructureTemplate.LoadFromNBT(loadtag);
            var schema = new SilverNBTLibrary.Structure.Schematic(structure.Width, structure.Height, structure.Length);
            form.Invoke((MethodInvoker)delegate ()
            {
                pgb.Maximum = structure.Width * structure.Height * structure.Length;
                pgb.Value = 0;
            });
            for (int x = 0; x < structure.Width; x++)
            {
                for (int y = 0; y < structure.Height; y++)
                {
                    for (int z = 0; z < structure.Length; z++)
                    {
                        var id = structure.GetBlockId(x, y, z);
                        var meta = structure.GetBlockMetadata(x, y, z);
                        schema.SetBlock(x, y, z, id, meta);
                        var te = structure.GetTileEntityTag(x, y, z);
                        if (te != null)
                            schema.AddTileEntityTag(x, y, z, te);
                        form.Invoke((MethodInvoker)delegate ()
                        {
                            pgb.Value++;
                        });
                    }
                }
            }

            foreach (NBTTagCompound entity in structure.Entities)
            {
                schema.Entities.Add(entity);
            }
            var nbt = schema.SaveToNBT();
            NBTFile.SaveToFile(Path.GetDirectoryName(file) + @"\" + Path.GetFileNameWithoutExtension(file) + ".schematic", nbt);
        }

    }
}
