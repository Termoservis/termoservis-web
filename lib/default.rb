# All files in the 'lib' directory will be loaded
# before nanoc starts compiling.

include Nanoc::Helpers::Rendering
include Nanoc::Helpers::Breadcrumbs
include Nanoc3::Helpers::HTMLEscape
include Nanoc3::Helpers::LinkTo

class CodeExampleFilter < Nanoc::Filter
  identifier :codeexample

  def run(content, params={})
    content.gsub(/(^`{3}\s*(\S*)\s*$([^`]*)^={4}\s*\s*(\S*)\s*$([^`]*)^`{3}\s*$)+?/m) {|match|
      lang_spec  = $2
      preview_block = $3
      code_block = $5

      replacement = '<div class="paper"><div class="paper-container">'

      if lang_spec && lang_spec.length > 0
        replacement << '<div class="type">'
        replacement << lang_spec.capitalize
        replacement << '</div>'
      end

      if preview_block && preview_block.length > 0
        replacement << '<div class="preview">'
        preview_block.gsub!("[:backtick:]", "`")
        replacement << preview_block.strip
        replacement << '</div>'
      end

      replacement << '<pre><code>'
      code_block.gsub!("[:backtick:]", "`")
      replacement << code_block.strip
      replacement << '</code></pre></div></div>'
    }
  end
end
