﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NzbDrone.Test.Common;

namespace NzbDrone.Common.Test
{
    [TestFixture]
    public class DirectoryLookupServiceFixture :TestBase<DirectoryLookupService>
    {
        private const string RECYCLING_BIN = "$Recycle.Bin";
        private const string SYSTEM_VOLUME_INFORMATION = "System Volume Information";
        private List<String> _folders;

        [SetUp]
        public void Setup()
        {
            _folders = new List<String>
            {
                RECYCLING_BIN,
                "Chocolatey",
                "Documents and Settings",
                "Dropbox",
                "Intel",
                "PerfLogs",
                "Program Files",
                "Program Files (x86)",
                "ProgramData",
                SYSTEM_VOLUME_INFORMATION,
                "Test",
                "Users",
                "Windows"
            };
        }

        private void SetupFolders(string root)
        {
            _folders.ForEach(e =>
            {
                e = Path.Combine(root, e);
            });
        }
            
        [Test]
        public void should_get_all_folder_for_none_root_path()
        {
            const string root = @"C:\Test\";
            SetupFolders(root);

            Mocker.GetMock<IDiskProvider>()
                .Setup(s => s.GetDirectories(It.IsAny<String>()))
                .Returns(_folders.ToArray());

            Subject.LookupSubDirectories(root).Should()
                   .HaveCount(_folders.Count);
        }

        [Test]
        public void should_not_contain_recycling_bin_for_root_of_drive()
        {
            const string root = @"C:\";
            SetupFolders(root);

            Mocker.GetMock<IDiskProvider>()
                .Setup(s => s.GetDirectories(It.IsAny<String>()))
                .Returns(_folders.ToArray());

            Subject.LookupSubDirectories(root).Should().NotContain(Path.Combine(root, RECYCLING_BIN));
        }

        [Test]
        public void should_not_contain_system_volume_information_for_root_of_drive()
        {
            const string root = @"C:\";
            SetupFolders(root);

            Mocker.GetMock<IDiskProvider>()
                .Setup(s => s.GetDirectories(It.IsAny<String>()))
                .Returns(_folders.ToArray());

            Subject.LookupSubDirectories(root).Should().NotContain(Path.Combine(root, SYSTEM_VOLUME_INFORMATION));
        }

        [Test]
        public void should_not_contain_recycling_bin_or_system_volume_information_for_root_of_drive()
        {
            const string root = @"C:\";
            SetupFolders(root);

            Mocker.GetMock<IDiskProvider>()
                .Setup(s => s.GetDirectories(It.IsAny<String>()))
                .Returns(_folders.ToArray());

            Subject.LookupSubDirectories(root).Should().HaveCount(_folders.Count - 2);
        }
    }
}
