from pathlib import Path


def main():
    with open('build.Lang.template.nuspec', 'r', encoding='utf-8') as template_file:
        template_content = template_file.read()

    resx_file_folder = Path('../src/Shared/HandyControl_Shared/Properties/Langs')
    for lang in [path.stem.lstrip('Lang.') for path in resx_file_folder.glob('Lang.*.resx')]:
        with open(f'build.Lang.{lang}.nuspec', 'w', encoding='utf-8') as nuspec_file:
            nuspec_file.write(template_content.format(lang=lang))


if __name__ == '__main__':
    main()
