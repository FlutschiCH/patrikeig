import paramiko
import os

class SSHHandler:
    @staticmethod
    def execute_command(host="10.20.110.7", user="root", password=None, key_filename=None, command="ss -tulpn", timeout=5):
        """Executes a command via SSH and returns stdout/stderr."""
        client = paramiko.SSHClient()
        client.set_missing_host_key_policy(paramiko.AutoAddPolicy())

        # Check default user SSH key locations if none provided
        if not key_filename and not password:
            default_rsa = os.path.expanduser("~/.ssh/id_rsa")
            default_ed25519 = os.path.expanduser("~/.ssh/id_ed25519")
            if os.path.exists(default_rsa):
                key_filename = default_rsa
            elif os.path.exists(default_ed25519):
                key_filename = default_ed25519

        try:
            client.connect(
                hostname=host,
                port=22,
                username=user,
                password=password if password else None,
                key_filename=key_filename if key_filename else None,
                timeout=timeout,
                look_for_keys=True,
                allow_agent=True
            )

            stdin, stdout, stderr = client.exec_command(command, timeout=timeout)
            out = stdout.read().decode('utf-8', errors='replace')
            err = stderr.read().decode('utf-8', errors='replace')
            exit_status = stdout.channel.recv_exit_status()
            client.close()

            return {
                "success": exit_status == 0,
                "command": command,
                "exit_status": exit_status,
                "stdout": out,
                "stderr": err
            }
        except paramiko.AuthenticationException:
            return {
                "success": False,
                "command": command,
                "exit_status": 1,
                "stdout": "",
                "stderr": "SSH Authentication Failed. Please supply valid password or SSH key in GUI."
            }
        except Exception as e:
            return {
                "success": False,
                "command": command,
                "exit_status": 1,
                "stdout": "",
                "stderr": f"SSH Connection Error: {str(e)}"
            }
