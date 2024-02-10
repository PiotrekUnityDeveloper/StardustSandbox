﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using PixelDust.Game.Constants;
using PixelDust.Game.Objects;

namespace PixelDust.Game.Databases
{
    public sealed class PAssetDatabase(ContentManager contentManager) : PGameObject
    {
        private enum AssetType
        {
            Texture,
            Font,
            Song,
            Sound,
            Shader
        }

        public Texture2D[] Textures => [.. this.textures.Values];
        public SpriteFont[] Fonts => [.. this.fonts.Values];
        public Song[] Songs => [.. this.songs.Values];
        public SoundEffect[] Sounds => [.. this.sounds.Values];
        public Effect[] Shaders => [.. this.shaders.Values];

        private readonly ContentManager _cm = contentManager;

        private readonly Dictionary<string, Texture2D> textures = [];
        private readonly Dictionary<string, SpriteFont> fonts = [];
        private readonly Dictionary<string, Song> songs = [];
        private readonly Dictionary<string, SoundEffect> sounds = [];
        private readonly Dictionary<string, Effect> shaders = [];

        protected override void OnAwake()
        {
            LoadShaders();
            LoadFonts();
            LoadGraphics();
            LoadSounds();
            LoadSongs();
        }

        #region LOAD
        private void LoadShaders()
        {
            AssetLoader(AssetType.Shader, PAssetsConstants.SHADERS_LENGTH, "shader_", PDirectoryConstants.ASSETS_SHADERS);
        }
        private void LoadFonts()
        {
            AssetLoader(AssetType.Font, PAssetsConstants.FONTS_LENGTH, "font_", PDirectoryConstants.ASSETS_FONTS);
        }
        private void LoadGraphics()
        {
            AssetLoader(AssetType.Texture, PAssetsConstants.GRAPHICS_BACKGROUND_LENGTH, "background_", Path.Combine(PDirectoryConstants.ASSETS_GRAPHICS, PDirectoryConstants.ASSETS_GRAPHICS_BACKGROUND));
            AssetLoader(AssetType.Texture, PAssetsConstants.GRAPHICS_BGOS_LENGTH, "bgos_", Path.Combine(PDirectoryConstants.ASSETS_GRAPHICS, PDirectoryConstants.ASSETS_GRAPHICS_BGOS));
            AssetLoader(AssetType.Texture, PAssetsConstants.GRAPHICS_EFFECTS_LENGTH, "effect_", Path.Combine(PDirectoryConstants.ASSETS_GRAPHICS, PDirectoryConstants.ASSETS_GRAPHICS_EFFECTS));
            AssetLoader(AssetType.Texture, PAssetsConstants.GRAPHICS_ELEMENTS_LENGTH, "element_", Path.Combine(PDirectoryConstants.ASSETS_GRAPHICS, PDirectoryConstants.ASSETS_GRAPHICS_ELEMENTS));
            AssetLoader(AssetType.Texture, PAssetsConstants.GRAPHICS_PARTICLES_LENGTH, "particle_", Path.Combine(PDirectoryConstants.ASSETS_GRAPHICS, PDirectoryConstants.ASSETS_GRAPHICS_PARTICLES));
        }
        private void LoadSounds()
        {
            AssetLoader(AssetType.Sound, PAssetsConstants.SOUNDS_LENGTH, "sound_", PDirectoryConstants.ASSETS_SOUNDS);
        }
        private void LoadSongs()
        {
            AssetLoader(AssetType.Song, PAssetsConstants.SONGS_LENGTH, "song_", PDirectoryConstants.ASSETS_SONGS);
        }
        #endregion

        #region GET
        public Texture2D GetTexture(string name)
        {
            return this.textures[name];
        }
        public SpriteFont GetFont(string name)
        {
            return this.fonts[name];
        }
        public Song GetSong(string name)
        {
            return this.songs[name];
        }
        public SoundEffect GetSound(string name)
        {
            return this.sounds[name];
        }
        public Effect GetShader(string name)
        {
            return this.shaders[name];
        }
        #endregion

        private void AssetLoader(AssetType assetType, int length, string prefix, string path)
        {
            int targetId;
            string targetName;
            string targetPath;

            for (int i = 0; i < length; i++)
            {
                targetId = i + 1;
                targetName = string.Concat(prefix, targetId);
                targetPath = Path.Combine(path, targetName);

                switch (assetType)
                {
                    case AssetType.Texture:
                        this.textures.Add(targetName, this._cm.Load<Texture2D>(targetPath));
                        break;

                    case AssetType.Font:
                        this.fonts.Add(targetName, this._cm.Load<SpriteFont>(targetPath));
                        break;

                    case AssetType.Song:
                        this.songs.Add(targetName, this._cm.Load<Song>(targetPath));
                        break;

                    case AssetType.Sound:
                        this.sounds.Add(targetName, this._cm.Load<SoundEffect>(targetPath));
                        break;

                    default:
                        return;
                }
            }
        }
    }
}