using AceJumpPackage.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AceJumpTests
{
    using AceJump;

    using Moq;

    [TestClass]
    public class JumpControlerTests
    {
        private JumpControler controler;

        private Mock<IAceJumpAdornment> adornmentMock;

        [TestInitialize]
        public void BeforeEachTest()
        {
            this.adornmentMock = new Mock<IAceJumpAdornment>();
            this.controler = new JumpControler(this.adornmentMock.Object);

            adornmentMock.Setup(a => a.OffsetKey).Returns(() =>null);
        }

        [TestMethod]
        public void if_jump_adorment_is_null_return_false()
        {
            JumpControler controler = new JumpControler(null);
            Assert.IsFalse(controler.ControlJump('2'));
        }

        [TestMethod]
        public void if_jumper_short_cut_pressed_call_show_jumper()
        {
            controler.ShowJumpEditor();

            adornmentMock.Verify(a => a.ShowSelector(), Times.Once());
        }

        [TestMethod]
        public void if_jumper_short_cut_pressed_calls_clear_Adroments()
        {
            adornmentMock.Setup(a => a.Active).Returns(true);
             controler.ShowJumpEditor();

            adornmentMock.Verify(a => a.ClearAdornments(), Times.Once());
        }

        [TestMethod]
        public void if_Active_and_no_key_passed_ignore_key()
        {
            adornmentMock.Setup(a => a.Active).Returns(true);
            var handled = controler.ControlJump(null);

            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void if_second_press_then_jump()
        {
            adornmentMock.Setup(a => a.Active).Returns(true);

            // first time activates letters to jump to
            var handled = controler.ControlJump('a');
            handled = controler.ControlJump('b');

            adornmentMock.Verify(a=>a.JumpTo("B"), Times.Once);
            adornmentMock.Verify(a=>a.ClearAdornments(), Times.Once);
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void if_first_press_then_highlight()
        {
            adornmentMock.Setup(a => a.Active).Returns(true);
          

            // first time activates letters to jump to
            var handled = controler.ControlJump('a');

            adornmentMock.Verify(a => a.HighlightLetter("A"), Times.Once);
            Assert.IsTrue(handled);
        }


        [TestMethod]
        public void if_second_press_and_multplekey_selector_then_wait()
        {
            adornmentMock.Setup(a => a.Active).Returns(true);
            adornmentMock.Setup(a => a.OffsetKey).Returns('X');

            // first time activates letters to jump to
            var handled = controler.ControlJump('a');
            handled = controler.ControlJump('X');

            adornmentMock.Verify(a => a.JumpTo("B"), Times.Never);
            adornmentMock.Verify(a => a.ClearAdornments(), Times.Never);
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void if_Third_press_after_multplekey_selector_then_jump()
        {
            adornmentMock.Setup(a => a.Active).Returns(true);
            adornmentMock.Setup(a => a.OffsetKey).Returns('X');

            // first time activates letters to jump to
            var handled = controler.ControlJump('a');
            handled = controler.ControlJump('X');
            handled = controler.ControlJump('b');

            adornmentMock.Verify(a => a.JumpTo("XB"), Times.Once);
            adornmentMock.Verify(a => a.ClearAdornments(), Times.Once);
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void if_Third_press_is_multikeyselector_after_multplekey_selector_then_jump()
        {
            adornmentMock.Setup(a => a.Active).Returns(true);
            adornmentMock.Setup(a => a.OffsetKey).Returns('X');

            // first time activates letters to jump to
            var handled = controler.ControlJump('a');
            handled = controler.ControlJump('X');
            handled = controler.ControlJump('X');

            adornmentMock.Verify(a => a.JumpTo("XX"), Times.Once);
            adornmentMock.Verify(a => a.ClearAdornments(), Times.Once);
            Assert.IsTrue(handled);
        }
    }
}
