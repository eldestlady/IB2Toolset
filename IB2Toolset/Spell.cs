﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
//using IceBlink;

namespace IB2Toolset
{
    /*public class Spells
    {
        public List<Spell> spellList = new List<Spell>();

        public Spells()
        {
        }
        public void saveSpellsFile(string filename)
        {
            string json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }
        public Spells loadSpellsFile(string filename)
        {
            Spells toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (Spells)serializer.Deserialize(file, typeof(Spells));
            }
            return toReturn;
        }
        public Spell getSpellByTag(string tag)
        {
            foreach (Spell s in spellList)
            {
                if (s.tag == tag) return s;
            }
            return null;
        }
        public Spell getSpellByName(string name)
        {
            foreach (Spell s in spellList)
            {
                if (s.name == name) return s;
            }
            return null;
        }
    }*/

    public class Spell
    {        
        public enum EffectType
        {
            Damage = 0, //usually persistent and negative for effect target
            Heal = 1,   //usually persistent and positive for effect target
            Buff = 2,   //usually temporary and positive for effect target
            Debuff = 3  //usually temporary and negative for effect target
        }

        #region Fields      
        private bool _usesTurnToActivate = true; //some spells, especially called via active traits, are meant to be used in the same turn such as Power Attack and Set Trap    
        private string _name = "newSpell"; //item name
        private string _tag = "newSpellTag"; //item unique tag name
        private string _spellImage = "sp_magebolt";
        private string _description = "";
        //private UsableInSituation useableInSituation = UsableInSituation.Always;
        private string _useableInSituation = "Always";        
        private string _spriteFilename = "none";
        private string _spriteEndingFilename = "none";
        private string _spellStartSound = "none";
        private string _spellEndSound = "none";
        private int _costSP = 0;
        private int _costHP = 0;
        private string _spellTargetType = "Enemy";
        //private TargetType spellTargetType = TargetType.Enemy;
        private string _spellEffectType = "Damage";
        //private EffectType spellEffectType = EffectType.Damage;
        private AreaOfEffectShape _aoeShape = AreaOfEffectShape.Circle;
        private int _aoeRadius = 0;
        private int _range = 0;
        //private ScriptSelectEditorReturnObject spellScript = new ScriptSelectEditorReturnObject();
        private string _spellScript = "none";
        private string _additionalCustomLogTextOnCast = "none";
        private string _spellEffectTag = "none";
        private List<EffectTagForDropDownList> _removeEffectTagList = new List<EffectTagForDropDownList>();
        private List<EffectTagForDropDownList> _spellEffectTagList = new List<EffectTagForDropDownList>();
        public List<LocalImmunityString> traitWorksOnlyWhen = new List<LocalImmunityString>();
        public List<LocalImmunityString> traitWorksNeverWhen = new List<LocalImmunityString>();
        private bool _isUsedForCombatSquareEffect = false;
        private int _castTimeInTurns = 0;
        private bool _canBeInterrupted = true;
        private bool _triggersAoO = true;

        #endregion

        #region Properties

        [CategoryAttribute("01 - Main"), DescriptionAttribute("if true, the use of this trait in combat will consume that player's turn. If false, the player will get to use this trait and continue their turn. Some traits are meant to be used in the same turn such as Power Attack and Set Trap.")]  
        public bool usesTurnToActivate  
        {  
             get { return _usesTurnToActivate; }  
             set { _usesTurnToActivate = value; }  
        }  

        [CategoryAttribute("02 - Target"), DescriptionAttribute("Does this spell apply effects to combat squares?")]  
         public bool isUsedForCombatSquareEffect
        {  
            get  
            {
                return _isUsedForCombatSquareEffect;  
            }  
            set  
            {  
                _isUsedForCombatSquareEffect = value;  
            }  
        }  

        [CategoryAttribute("01 - Main"), DescriptionAttribute("Name of the Spell")]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Tag of the Spell (Must be unique)")]
        public string tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Image icon of the Spell")]
        public string spellImage
        {
            get
            {
                return _spellImage;
            }
            set
            {
                _spellImage = value;
            }
        }
        [Editor(typeof(MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Detailed description of spell with some stats and cost as well")]
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        /*[CategoryAttribute("01 - Main"), DescriptionAttribute("When can this be used: Always means that it can be used in combat and on the main maps, Passive means that it is always on and doesn't need to be activated.")]
        public UsableInSituation UseableInSituation
        {
            get { return useableInSituation; }
            set { useableInSituation = value; }
        }*/
        [CategoryAttribute("01 - Main"), DescriptionAttribute("When can this be used: Always means that it can be used in combat and on the main maps, Passive means that it is always on and doesn't need to be activated.")]
        public string useableInSituation
        {
            get { return _useableInSituation; }
            set { _useableInSituation = value; }
        }
        [Browsable(true), TypeConverter(typeof(SpriteConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Sprite to use for the effect (Sprite Filename with no extension)")]
        public string spriteFilename
        {
            get
            {
                return _spriteFilename;
            }
            set
            {
                _spriteFilename = value;
            }
        }
        [Browsable(true), TypeConverter(typeof(SpriteConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Sprite to use for the ending effect (Sprite Filename with no extension)")]
        public string spriteEndingFilename
        {
            get
            {
                return _spriteEndingFilename;
            }
            set
            {
                _spriteEndingFilename = value;
            }
        }
        [Browsable(true), TypeConverter(typeof(SoundConverter))]
        [CategoryAttribute("01- Main"), DescriptionAttribute("Filename of sound to play when the spell starts (no extension)")]
        public string spellStartSound
        {
            get { return _spellStartSound; }
            set { _spellStartSound = value; }
        }
        [Browsable(true), TypeConverter(typeof(SoundConverter))]
        [CategoryAttribute("01- Main"), DescriptionAttribute("Filename of sound to play when the spell ends (no extension)")]
        public string spellEndSound
        {
            get { return _spellEndSound; }
            set { _spellEndSound = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("How much SP this Spell costs")]
        public int costSP
        {
            get
            {
                return _costSP;
            }
            set
            {
                _costSP = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("How much HP this Spell costs")]
        public int costHP
        {
            get
            {
                return _costHP;
            }
            set
            {
                _costHP = value;
            }
        }
        /*
        private int _castTimeInTurns = 0;
        private bool _canBeInterrupted = true;
        private bool _triggersAoO = true;
        */
        [CategoryAttribute("01 - Main"), DescriptionAttribute("How many extra turns to spend casting; 0 is spell that is cast the same turn as it is selected")]
        public int castTimeInTurns
        {
            get
            {
                return _castTimeInTurns;
            }
            set
            {
                _castTimeInTurns = value;
            }
        }

        [CategoryAttribute("01 - Main"), DescriptionAttribute("When set to true, this spell can be interrupted by damage taken while casting (a will save is allowed against the interruption, getting more difficult, the more damage has been taken)")]
        public bool canBeInterrupted
        {
            get
            {
                return _canBeInterrupted;
            }
            set
            {
                _canBeInterrupted = value;
            }
        }

        [CategoryAttribute("01 - Main"), DescriptionAttribute("When set to true, enemies in melee range get free attacks on the caster during the casting procress.")]
        public bool triggersAoO
        {
            get
            {
                return _triggersAoO;
            }
            set
            {
                _triggersAoO = value;
            }
        }
        /*[CategoryAttribute("02 - Target"), DescriptionAttribute("The type of target for this spell")]
        public TargetType SpellTargetType
        {
            get
            {
                return spellTargetType;
            }
            set
            {
                spellTargetType = value;
            }
        }*/
        [CategoryAttribute("02 - Target"), DescriptionAttribute("The type of target for this spell")]
        public string spellTargetType
        {
            get
            {
                return _spellTargetType;
            }
            set
            {
                _spellTargetType = value;
            }
        }
        /*[CategoryAttribute("03 - Effect"), DescriptionAttribute("damage = persistent, negative; heal = persistent, positive; buff = temporary, positive; debuff = temporary, negative")]
        public EffectType SpellEffectType
        {
            get
            {
                return spellEffectType;
            }
            set
            {
                spellEffectType = value;
            }
        }*/
        [CategoryAttribute("03 - Effect"), DescriptionAttribute("damage = persistent, negative; heal = persistent, positive; buff = temporary, positive; debuff = temporary, negative")]
        public string spellEffectType
        {
            get
            {
                return _spellEffectType;
            }
            set
            {
                _spellEffectType = value;
            }
        }
        [CategoryAttribute("02 - Target"), DescriptionAttribute("the shape of the AoE")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AreaOfEffectShape aoeShape
        {
            get
            {
                return _aoeShape;
            }
            set
            {
                _aoeShape = value;
            }
        }
        [CategoryAttribute("02 - Target"), DescriptionAttribute("the radius of the AoE")]
        public int aoeRadius
        {
            get
            {
                return _aoeRadius;
            }
            set
            {
                _aoeRadius = value;
            }
        }
        [CategoryAttribute("02 - Target"), DescriptionAttribute("the range allowed to the center or beginning of the AoE")]
        public int range
        {
            get
            {
                return _range;
            }
            set
            {
                _range = value;
            }
        }
        /*[CategoryAttribute("01 - Main"), DescriptionAttribute("the script to use for this Spell")]
        [Editor(typeof(ScriptSelectEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ScriptSelectEditorReturnObject SpellScript
        {
            get { return spellScript; }
            set { spellScript = value; }
        }*/
        [CategoryAttribute("01 - Main"), DescriptionAttribute("the script to use for this Spell (leave as 'none' to use spellEffectTag instead)")]
        //[Editor(typeof(ScriptSelectEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string spellScript
        {
            get { return _spellScript; }
            set { _spellScript = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("this text appears additionally in log when the spell is cast")]
        //[Editor(typeof(ScriptSelectEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string additionalCustomLogTextOnCast
        {
            get { return _additionalCustomLogTextOnCast; }
            set { _additionalCustomLogTextOnCast = value; }
        }
        [Browsable(true), TypeConverter(typeof(EffectTagTypeConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("the effect to use for this Spell")]
        public string spellEffectTag
        {
            get { return _spellEffectTag; }
            set { _spellEffectTag = value; }
        }
        //[Browsable(true), TypeConverter(typeof(EffectTagTypeConverter))]
        [CategoryAttribute("05 - Spell/Effect System"), DescriptionAttribute("List of EffectTags that will be removed from the target when this spell is used (used for dispell magic, free action, neutralize poison, etc.)")]
        public List<EffectTagForDropDownList> removeEffectTagList
        {
            get { return _removeEffectTagList; }
            set { _removeEffectTagList = value; }
        }
        //[Browsable(true), TypeConverter(typeof(EffectTagTypeConverter))]
        [CategoryAttribute("05 - Spell/Effect System"), DescriptionAttribute("List of EffectTags of this spell")]
        public List<EffectTagForDropDownList> spellEffectTagList
        {
            get { return _spellEffectTagList; }
            set { _spellEffectTagList = value; }
        }
        #endregion

        public Spell()
        {            
        }
        public override string ToString()
        {
            return name;
        }
        public Spell ShallowCopy()
        {
            return (Spell)this.MemberwiseClone();
        }
        public Spell DeepCopy()
        {
            Spell other = (Spell)this.MemberwiseClone();
            other.removeEffectTagList = new List<EffectTagForDropDownList>();
            foreach (EffectTagForDropDownList s in this.removeEffectTagList)
            {
                other.removeEffectTagList.Add(s);
            }
            other.spellEffectTagList = new List<EffectTagForDropDownList>();
            foreach (EffectTagForDropDownList s in this.spellEffectTagList)
            {
                other.spellEffectTagList.Add(s);
            }
            other.traitWorksNeverWhen = new List<LocalImmunityString>();
            foreach (LocalImmunityString s in this.traitWorksNeverWhen)
            {
                other.traitWorksNeverWhen.Add(s);
            }
            other.traitWorksOnlyWhen = new List<LocalImmunityString>();
            foreach (LocalImmunityString s in this.traitWorksOnlyWhen)
            {
                other.traitWorksOnlyWhen.Add(s);
            }


            return other;
        }
    }
}
