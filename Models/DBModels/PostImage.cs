﻿using System;
using System.Collections.Generic;

#nullable disable

namespace petsLighthouseAPI
{
    public partial class PostImage
    {
        public int IdImage { get; set; }
        public int IdPostPet { get; set; }
        public string Url { get; set; }

        public virtual PostPet IdPostPetNavigation { get; set; }
    }
}
